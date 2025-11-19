using System.Collections.Generic;
using UnityEngine;

public class GhostPoolManager : MonoBehaviour
{
    public static GhostPoolManager Instance;

    [Header("Pool Settings")]
    public GameObject ghostPrefab;
    public int poolSize = 12;
    public Transform poolHolder;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Pre-fill pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject g = Instantiate(
                ghostPrefab,
                poolHolder ? poolHolder.position : Vector3.zero,
                Quaternion.identity,
                poolHolder
            );

            g.SetActive(false);      // OnEnable will NOT run yet
            pool.Enqueue(g);
        }
    }


    public GameObject GetGhost()
    {
        GameObject g;

        if (pool.Count > 0)
        {
            g = pool.Dequeue();
        }
        else
        {
            Debug.LogWarning("Ghost pool empty — expanding.");

            g = Instantiate(
                ghostPrefab,
                poolHolder ? poolHolder.position : Vector3.zero,
                Quaternion.identity,
                poolHolder
            );
        }

        // Activating triggers GhostController.OnEnable()
        g.SetActive(true);
        return g;
    }


    public void ReturnGhost(GameObject ghost)
    {
        // Disable first - OnDisable() unregisters ghost
        ghost.SetActive(false);

        // Reset internal ghost logic AFTER disable
        GhostController gc = ghost.GetComponent<GhostController>();
        GhostData gd = ghost.GetComponent<GhostData>();

        if (gc != null)
            gc.ForceReset();   // Safe reset

        // If golden ghost - notify spawner to unlock golden slot
        if (gd != null && gd.isGolden)
        {
            GhostSpawner.Instance.NotifyGoldenReturned();
        }

        // Re-parent back to pool
        if (poolHolder)
            ghost.transform.SetParent(poolHolder, false);

        ghost.transform.position = poolHolder ? poolHolder.position : Vector3.zero;

        // Enqueue back into pool
        pool.Enqueue(ghost);
        GhostManager.Unregister(gc);
    }
}
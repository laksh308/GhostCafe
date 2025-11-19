using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class GhostSpawner : MonoBehaviour
{
    public static GhostSpawner Instance;

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Spawn Tuning")]
    public float baseSpawnInterval = 3f;
    public float spawnVariance = 1f;
    public int maxActiveGhosts = 6;
    public float initialDelay = 0.5f;

    private float timer = 0f;
    private float nextInterval = 0f;

    // Golden ghost lock (1 golden at a time)
    private bool goldenActive = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timer = initialDelay;
        ScheduleNext();
    }

    void Update()
    {
        // Prevent spawn if ghost count full
        if (GhostManager.Count >= maxActiveGhosts)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnOne();
            ScheduleNext();
        }
    }


    private void ScheduleNext()
    {
        float mult = UpgradeManager.Instance != null
            ? UpgradeManager.Instance.GetMultiplier(UpgradeType.PhantomPull)
            : 1f;

        float interval = baseSpawnInterval / Mathf.Max(mult, 0.1f);

        nextInterval = interval + Random.Range(-spawnVariance, spawnVariance);
        nextInterval = Mathf.Clamp(nextInterval, 0.25f, 999f);

        timer = nextInterval;
    }


    private void SpawnOne()
    {
        if (spawnPoints == null || spawnPoints.Count == 0)
            return;

        // Pick random spawn point
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Add small random offset for natural feeling
        Vector3 pos = sp.position + new Vector3(
            Random.Range(-0.6f, 0.6f),
            Random.Range(-0.4f, 0.4f),
            0f
        );

        // Get pooled ghost
        GameObject ghost = GhostPoolManager.Instance.GetGhost();
        ghost.transform.SetParent(null); // detach from pool

        GhostController gc = ghost.GetComponent<GhostController>();
        GhostData gd = ghost.GetComponent<GhostData>();

        // Set home position + place ghost there
        gc.SetHomePosition(pos);
        ghost.transform.position = pos;

        gc.PlaySpawnAnimation();
        float goldenChance = UpgradeManager.Instance.GetMultiplier(UpgradeType.HauntingReputation);

        bool spawnGolden = Random.value < goldenChance && !goldenActive;

        gd.SetAsGolden(spawnGolden);

        if (spawnGolden)
            goldenActive = true;


        GhostManager.Register(gc);
    }

    public void NotifyGoldenReturned()
    {
        goldenActive = false;
    }
}
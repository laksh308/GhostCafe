using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Room Float Settings")]
    public GameObject Room;
    public float amplitude = 0.05f;
    public float frequency = 0.5f;

    private Vector3 roomStartPos;

    [Header("Shooting Star Settings")]
    public GameObject shootingStarPrefab;
    public Transform startPointA;
    public Transform endPointA;
    public Transform startPointB;
    public Transform endPointB;
    public float minSpawnDelay = 10f;
    public float maxSpawnDelay = 25f;
    public float starSpeed = 4f;
    [Range(0f, 1f)] public float routeAChance = 0.5f; // Probability for route A

    private float nextSpawnTime;

    void Start()
    {
        roomStartPos = Room.transform.position;
        ScheduleNextStar();
    }

    void Update()
    {
        // Floating room
        Room.transform.position = roomStartPos + new Vector3(
            0f,
            Mathf.Sin(Time.time * frequency) * amplitude,
            0f
        );

        // Shooting star spawner
        if (Time.time >= nextSpawnTime)
        {
            SpawnShootingStar();
            ScheduleNextStar();
        }
    }

    void SpawnShootingStar()
    {
        if (shootingStarPrefab == null) return;

        bool useRouteA = Random.value < routeAChance;

        Transform start = useRouteA ? startPointA : startPointB;
        Transform end = useRouteA ? endPointA : endPointB;

        if (start == null || end == null)
        {
            Debug.LogWarning("[EnvironmentManager] Missing start or end point for shooting star.");
            return;
        }

        GameObject star = Instantiate(shootingStarPrefab, start.position, Quaternion.identity);

        StartCoroutine(MoveStar(star, start.position, end.position));
    }

    IEnumerator MoveStar(GameObject star, Vector3 startPos, Vector3 endPos)
    {
        float distance = Vector3.Distance(startPos, endPos);
        float duration = distance / starSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (star == null) yield break;
            star.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (star != null)
            Destroy(star);
    }

    void ScheduleNextStar()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
    }
}
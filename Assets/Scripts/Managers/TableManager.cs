using UnityEngine;
using System.Collections.Generic;

public class TableManager : MonoBehaviour
{
    public static TableManager Instance;

    [Header("Table Setup")]
    public GameObject tablePrefab;

    [Tooltip("Order: bottomMiddle, bottomLeft, bottomRight, topMiddle, topLeft, topRight")]
    public Transform[] tablePositions;

    [Header("Unlock Settings")]
    [Range(0, 6)]
    public int numberOfTablesUnlocked = 1;

    private int lastUnlocked = -1;
    private readonly List<GameObject> activeTables = new List<GameObject>();

    // Collect all table seats here
    public readonly List<TableSeat> allSeats = new List<TableSeat>();

    // Balanced table unlock pattern
    private readonly int[][] unlockPatterns =
    {
        new int[] {},                     // 0
        new int[] { 0 },                  // 1
        new int[] { 1, 2 },               // 2
        new int[] { 1, 0, 2 },            // 3
        new int[] { 1, 0, 2, 3 },         // 4
        new int[] { 1, 0, 2, 4, 5 },      // 5
        new int[] { 1, 0, 2, 4, 3, 5 },   // 6
    };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshTables();
    }

    void Update()
    {
        // Only rebuild tables if inspector value changed
        if (numberOfTablesUnlocked != lastUnlocked)
            RefreshTables();
    }

    void RefreshTables()
    {
        lastUnlocked = numberOfTablesUnlocked;

        // Destroy old tables
        foreach (var t in activeTables)
            if (t != null) Destroy(t);

        activeTables.Clear();
        allSeats.Clear();

        int[] pattern = unlockPatterns[numberOfTablesUnlocked];

        foreach (int index in pattern)
        {
            GameObject tableObj = Instantiate(
                tablePrefab,
                tablePositions[index].position,
                Quaternion.identity,
                transform
            );

            activeTables.Add(tableObj);

            // Register seats from this table
            TableSeat[] seats = tableObj.GetComponentsInChildren<TableSeat>(true);
            allSeats.AddRange(seats);
        }
    }

    public TableSeat FindClosestAvailableSeat(Vector3 worldPos, float radius)
    {
        TableSeat best = null;
        float bestDist = Mathf.Infinity;

        foreach (var seat in allSeats)
        {
            if (seat == null || seat.isOccupied) continue;

            float dist = Vector3.Distance(worldPos, seat.seatPoint.position);
            if (dist < radius && dist < bestDist)
            {
                best = seat;
                bestDist = dist;
            }
        }

        return best;
    }
}
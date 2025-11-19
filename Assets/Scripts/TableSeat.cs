using UnityEngine;

/// <summary>
/// Represents a single seat on a table. Handles occupancy, snapping,
/// progress bar updates, and seating logic.
/// </summary>
public class TableSeat : MonoBehaviour
{
    [Header("Seat Setup")]
    public Transform seatPoint;                // Exact position where ghost snaps
    public float acceptRadius = 0.8f;          // Drag-drop distance allowed

    [Header("State")]
    public bool isOccupied = false;
    public GhostController seatedGhost;


    public bool TrySeat(GhostController ghost)
    {
        if (ghost == null || isOccupied)
            return false;

        float dist = Vector3.Distance(ghost.transform.position, seatPoint.position);
        if (dist > acceptRadius)
            return false;

        // Mark seat as occupied
        isOccupied = true;
        seatedGhost = ghost;
        ghost.currentSeat = this;

        // Tell ghost it has been seated
        ghost.OnSeated(this);

        return true;
    }


    public void Unseat()
    {
        if (seatedGhost != null)
        {
            seatedGhost.currentSeat = null;
            seatedGhost = null;
        }

        isOccupied = false;
    }

    public bool IsNear(Vector3 worldPos, float radius)
    {
        return !isOccupied &&
               Vector3.Distance(worldPos, seatPoint.position) <= radius;
    }
}
/*using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform stopPoint;                         // Front of the queue (main table)
    public List<Transform> queuePoints;                 // Visual positions (10 for now)
    public bool isGoldenGhostActive = false;

    private List<GameObject> queuedGhosts = new List<GameObject>();

    // Check if queue is full based on current upgrade
    public bool IsQueueFull()
    {
        int maxQueueSize = Mathf.RoundToInt(UpgradeManager.Instance.GetMultiplier(UpgradeType.FloatingTable));
        return queuedGhosts.Count >= maxQueueSize;
    }

    // Where should the next ghost move to
    public Vector3 GetNextAvailablePosition()
    {
        if (queuedGhosts.Count == 0)
            return stopPoint.position;

        if (queuedGhosts.Count <= queuePoints.Count)
            return queuePoints[queuedGhosts.Count - 1].position;

        // Ghosts beyond visual range stack downward offscreen
        return queuePoints[queuePoints.Count - 1].position + new Vector3(0f, -0.5f * (queuedGhosts.Count - queuePoints.Count), 0f);
    }

    // Add a ghost to the queue
    public void AddGhost(GameObject ghost)
    {
        queuedGhosts.Add(ghost);
    }

    // Remove the actual ghost (served or timed out)
    public void RemoveServedGhost(GameObject ghost)
    {
        if (!queuedGhosts.Contains(ghost))
        {
            Debug.LogWarning($"Tried to remove ghost not in queue: {ghost.name}");
            return;
        }

        queuedGhosts.Remove(ghost);

        if (ghost.GetComponent<GhostData>().isGolden)
            isGoldenGhostActive = false;

        // Reassign target positions to shift the queue forward
        for (int i = 0; i < queuedGhosts.Count; i++)
        {
            Vector3 newTarget;

            if (i == 0)
                newTarget = stopPoint.position;
            else if (i - 1 < queuePoints.Count)
                newTarget = queuePoints[i - 1].position;
            else
                newTarget = queuePoints[queuePoints.Count - 1].position + new Vector3(0f, -0.5f * (i + 1 - queuePoints.Count), 0f);

            queuedGhosts[i].GetComponent<FloatDown>().MoveTo(newTarget);
        }
    }

    // For UI or debugging
    public int GetCurrentQueueSize()
    {
        return queuedGhosts.Count;
    }

    public int GetMaxQueueSize()
    {
        return Mathf.RoundToInt(UpgradeManager.Instance.GetMultiplier(UpgradeType.FloatingTable));
    }
}*/
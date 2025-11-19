using System.Collections.Generic;
using UnityEngine;

public static class GhostManager
{
    // Store controllers (safer & more useful than GameObjects)
    private static readonly HashSet<GhostController> activeGhosts = new HashSet<GhostController>();

    // Register ghost when it becomes active
    public static void Register(GhostController g)
    {
        if (g != null)
            activeGhosts.Add(g);
    }

    // Unregister ghost when it goes to pool / destroyed
    public static void Unregister(GhostController g)
    {
        if (g != null)
            activeGhosts.Remove(g);
    }

    // Number of active ghosts alive in world
    public static int Count => activeGhosts.Count;

    // Reset automatically when game starts or scene reloads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        activeGhosts.Clear();
    }
}
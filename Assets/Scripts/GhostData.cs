using UnityEngine;

public class GhostData : MonoBehaviour
{
    public bool isGolden;
    public float waitTime;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetAsGolden(bool value)
    {
        isGolden = value;
        waitTime = value ? 3f : 10f;

        if (sr != null)
            sr.color = value ? Color.yellow : Color.white;
    }
}
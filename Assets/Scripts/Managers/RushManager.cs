using UnityEngine;

public class RushManager : MonoBehaviour
{
    public static RushManager Instance;

    public float rushMeter = 0f;
    public float rushThreshold = 500f;
    public float rushDuration = 10f;
    public float pointsPerGhost = 20f;

    private bool isRushActive = false;
    private float rushTimer = 0f;

    public delegate void RushEvent();
    public static event RushEvent OnRushStarted;
    public static event RushEvent OnRushEnded;
    public static event RushEvent OnRushMeterChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (isRushActive)
        {
            rushTimer -= Time.deltaTime;
            if (rushTimer <= 0f)
                EndRushMode();
        }
    }

    public void AddToMeter(float amount)
    {
        if (isRushActive) return;

        rushMeter += amount;
        rushMeter = Mathf.Clamp(rushMeter, 0f, rushThreshold);
        OnRushMeterChanged?.Invoke();

        if (rushMeter >= rushThreshold)
        {
            // UI should now allow activation
        }
    }

    public void ActivateRush()
    {
        if (isRushActive || rushMeter < rushThreshold) return;

        isRushActive = true;
        rushTimer = rushDuration;
        OnRushStarted?.Invoke();
    }

    private void EndRushMode()
    {
        isRushActive = false;
        rushMeter = 0f;
        OnRushEnded?.Invoke();
        OnRushMeterChanged?.Invoke();
    }

    public bool IsRushActive()
    {
        return isRushActive;
    }

    public float GetMultiplier()
    {
        return isRushActive ? 3f : 1f;
    }

    public float GetMeterPercent()
    {
        return rushMeter / rushThreshold;
    }
}
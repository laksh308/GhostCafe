using UnityEngine;
using System.Collections;
/*public class GhostInteractable : MonoBehaviour
{
    public bool canTimeout = false;
    public bool canBeServed = false;
    public bool isAtStopPoint = false;
    private bool isServed = false;
    private GhostData ghostData;

    private float timeWaited = 0f;

    void Start()
    {
        ghostData = GetComponent<GhostData>();
    }

    void Update()
    {
        if (!canTimeout || !isAtStopPoint || isServed) return;

        timeWaited += Time.deltaTime;

        if (timeWaited >= ghostData.waitTime)
        {
            isServed = true;

            FindObjectOfType<QueueManager>().RemoveServedGhost(gameObject);
            FindObjectOfType<GhostPoolManager>().ReturnGhost(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (isServed || !canBeServed) return;

        float distToStop = Vector3.Distance(transform.position, FindObjectOfType<QueueManager>().stopPoint.position);
        if (distToStop > 0.1f) return;

        isServed = true;

        float baseReward = ghostData.isGolden ? 1000 : 200;
        float upgradeMultiplier = UpgradeManager.Instance.GetMultiplier(UpgradeType.BlessedCauldron);
        float rushMultiplier = RushManager.Instance.GetMultiplier();

        int finalReward = Mathf.RoundToInt(baseReward * upgradeMultiplier * rushMultiplier);

        CurrencyManager.Instance.AddSpiritPoints(finalReward);
        UIManager.Instance.ShowPopup(transform.position, "+" + finalReward);
        SFXManager.Instance.PlayServe(ghostData.isGolden);
        HapticManager.VibrateLight();

        RushManager.Instance.AddToMeter(ghostData.isGolden ? 30f : 10f);

        FindObjectOfType<QueueManager>().RemoveServedGhost(gameObject);
        StartCoroutine(FadeAndReturn());
    }

    IEnumerator FadeAndReturn()
    {
        Vector3 originalScale = transform.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        transform.localScale = originalScale;
        FindObjectOfType<GhostPoolManager>().ReturnGhost(gameObject);
    }

    public void ResetState()
    {
        isServed = false;
        canBeServed = false;
        canTimeout = false;
        timeWaited = 0f;
        transform.localScale = Vector3.one;
    }
}
*/
public class HapticManager : MonoBehaviour
{
    private static float lastVibrationTime = 0f;
    private static float cooldown = 0.2f;

    public static void VibrateLight()
    {
        if (Time.time - lastVibrationTime > cooldown)
        {
            Handheld.Vibrate();
            lastVibrationTime = Time.time;
        }
    }
}
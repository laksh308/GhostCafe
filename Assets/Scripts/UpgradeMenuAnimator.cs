using UnityEngine;
using System.Collections;

public class UpgradeMenuAnimator : MonoBehaviour
{
    public RectTransform upgradePanel;
    public RectTransform menuPanel;

    public float slideDistance = 300f;
    public float duration = 0.3f;

    private bool isOpen = false;
    private Vector2 upgradeStartPos, menuStartPos;

    void Start()
    {
        upgradeStartPos = upgradePanel.anchoredPosition;
        menuStartPos = menuPanel.anchoredPosition;

        upgradePanel.gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        StopAllCoroutines();
        StartCoroutine(SlideUI(!isOpen));
        isOpen = !isOpen;
    }

    IEnumerator SlideUI(bool slideUp)
    {
        float time = 0f;

        Vector2 upgradeTarget = upgradeStartPos + Vector2.up * (slideUp ? slideDistance : 0);
        Vector2 menuTarget = menuStartPos + Vector2.up * (slideUp ? slideDistance : 0);

        Vector2 upgradeInitial = upgradePanel.anchoredPosition;
        Vector2 menuInitial = menuPanel.anchoredPosition;

        if (slideUp)
        {
            upgradePanel.gameObject.SetActive(true);
            TapInputBlocker.BlockInput = true;
        }

        while (time < duration)
        {
            float t = time / duration;
            upgradePanel.anchoredPosition = Vector2.Lerp(upgradeInitial, upgradeTarget, t);
            menuPanel.anchoredPosition = Vector2.Lerp(menuInitial, menuTarget, t);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        upgradePanel.anchoredPosition = upgradeTarget;
        menuPanel.anchoredPosition = menuTarget;

        if (!slideUp)
        {
            upgradePanel.gameObject.SetActive(false);
            TapInputBlocker.BlockInput = false;
        }
    }
}

public static class TapInputBlocker
{
    public static bool BlockInput = false;
}
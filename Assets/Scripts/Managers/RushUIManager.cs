using UnityEngine;
using UnityEngine.UI;

public class RushUIManager : MonoBehaviour
{
    public Slider rushMeterSlider;     // Reference to Slider
    public GameObject rushButton;      // Button to activate Rush

    private void OnEnable()
    {
        RushManager.OnRushMeterChanged += UpdateUI;
        RushManager.OnRushStarted += HideButton;
        RushManager.OnRushEnded += UpdateUI;
    }

    private void OnDisable()
    {
        RushManager.OnRushMeterChanged -= UpdateUI;
        RushManager.OnRushStarted -= HideButton;
        RushManager.OnRushEnded -= UpdateUI;
    }

    void UpdateUI()
    {
        float percent = RushManager.Instance.GetMeterPercent();
        rushMeterSlider.value = percent;  // Update slider
        rushButton.SetActive(percent >= 1f && !RushManager.Instance.IsRushActive());
    }

    void HideButton()
    {
        rushButton.SetActive(false);
    }

    public void OnRushButtonPressed()
    {
        RushManager.Instance.ActivateRush();
    }
}
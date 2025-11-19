using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class UpgradeUIItem : MonoBehaviour
{
    public UpgradeType upgradeType;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public Button upgradeButton;

    public int baseCost = 1000;
    public float costMultiplier = 1.7f;

    private void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgradeClicked);
        RefreshUI();
    }

    public void RefreshUI()
    {
        int level = UpgradeManager.Instance.GetLevel(upgradeType);
        int max = UpgradeManager.Instance.GetMaxLevel(upgradeType);
        int cost = GetUpgradeCost(level);

        titleText.text = upgradeType.ToString();
        levelText.text = "Lv. " + level + " / " + max;

        bool canUpgrade = level < max;

        costText.text = canUpgrade ? $"Cost: {cost}" : "MAXED";
        upgradeButton.interactable = canUpgrade;
    }

    public void OnUpgradeClicked()
    {
        int level = UpgradeManager.Instance.GetLevel(upgradeType);
        int cost = GetUpgradeCost(level);

        if (UpgradeManager.Instance.CanUpgrade(upgradeType, cost))
        {
            UpgradeManager.Instance.Upgrade(upgradeType, cost);

            SFXManager.Instance.UpgradeSound();

            StartCoroutine(PunchUpgradeEffect());
            RefreshUI();
        }
    }

    private int GetUpgradeCost(int level)
    {
        float raw = baseCost * Mathf.Pow(costMultiplier, level - 1);
        return Mathf.RoundToInt(raw / 50f) * 50;
    }
    IEnumerator PunchUpgradeEffect()
    {
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale * 1.1f;

        yield return new WaitForSeconds(0.1f);

        transform.localScale = originalScale;
    }

}
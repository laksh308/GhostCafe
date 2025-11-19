using UnityEngine;
using System.Collections.Generic;

public enum UpgradeType
{
    SpiritCall,
    PhantomPull,
    FloatingTable,
    HauntingReputation,
    BlessedCauldron
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    private Dictionary<UpgradeType, int> upgradeLevels = new Dictionary<UpgradeType, int>();
    private Dictionary<UpgradeType, int> maxLevels = new Dictionary<UpgradeType, int>()
    {
        { UpgradeType.SpiritCall, 5 },
        { UpgradeType.PhantomPull, 5 },
        { UpgradeType.FloatingTable, 5 },
        { UpgradeType.HauntingReputation, 5 },
        { UpgradeType.BlessedCauldron, 5 }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitUpgrades()
    {
        foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
        {
            string key = "Upgrade_" + type.ToString();
            if (PlayerPrefs.HasKey(key))
                upgradeLevels[type] = PlayerPrefs.GetInt(key);
            else
                upgradeLevels[type] = 1;
        }
    }

    public int GetLevel(UpgradeType type)
    {
        return upgradeLevels[type];
    }

    public int GetMaxLevel(UpgradeType type)
    {
        return maxLevels[type];
    }

    public bool IsMaxLevel(UpgradeType type)
    {
        return GetLevel(type) >= GetMaxLevel(type);
    }

    public float GetProgressPercent(UpgradeType type)
    {
        return (float)(GetLevel(type) - 1) / (GetMaxLevel(type) - 1);
    }

    public bool CanUpgrade(UpgradeType type, int cost)
    {
        return upgradeLevels[type] < maxLevels[type] && CurrencyManager.Instance.GetSpiritPoints() >= cost;
    }

    public void Upgrade(UpgradeType type, int cost)
    {
        if (CanUpgrade(type, cost))
        {
            CurrencyManager.Instance.SpendSpiritPoints(cost);
            upgradeLevels[type]++;
        }
    }

    public float GetMultiplier(UpgradeType type)
    {
        int level = GetLevel(type);

        switch (type)
        {
            case UpgradeType.SpiritCall:
                return level;
            case UpgradeType.PhantomPull:
                return Mathf.Lerp(6f, 3.5f, (level - 1) / 4f);
            case UpgradeType.FloatingTable:
                return 10 + (level - 1) * 5;
            case UpgradeType.HauntingReputation:
                return Mathf.Lerp(0.15f, 0.25f, (level - 1) / 4f);
            case UpgradeType.BlessedCauldron:
                return 1f + (0.1f * (level - 1));
            default:
                return 1f;
        }
    }

    public void SaveUpgrades()
    {
        foreach (var pair in upgradeLevels)
        {
            PlayerPrefs.SetInt("Upgrade_" + pair.Key.ToString(), pair.Value);
        }
    }
}
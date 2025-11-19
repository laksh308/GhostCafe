using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int spiritPoints;
    public TMP_Text spiritPointsText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadCurrency();
    }

    public void AddSpiritPoints(int amount)
    {
        spiritPoints += amount;
        Debug.Log("Spirit Points: " + spiritPoints);
        UpdateText();
    }

    public int GetSpiritPoints()
    {
        return spiritPoints;
    }

    public void SpendSpiritPoints(int amount)
    {
        spiritPoints -= amount;
        UpdateText();
    }

    private void UpdateText()
    {
        if (spiritPointsText != null)
            spiritPointsText.text = spiritPoints.ToString();
    }

    public void SaveCurrency()
    {
        PlayerPrefs.SetInt("SpiritPoints", spiritPoints);
    }

    public void LoadCurrency()
    {
        spiritPoints = PlayerPrefs.GetInt("SpiritPoints", 0);
        UpdateText();
    }
}
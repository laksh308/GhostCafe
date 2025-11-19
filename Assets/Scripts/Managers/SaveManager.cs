using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        CurrencyManager.Instance.SaveCurrency();
        UpgradeManager.Instance.SaveUpgrades();
    }
}
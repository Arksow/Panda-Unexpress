using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private int baseUpgradeCost = 100;
    private int sugarUpgradeCost = 100;

    [SerializeField] private GameObject baseUpgradeButton;
    [SerializeField] private GameObject sugarUpgradeButton;

    public void BuyBaseUpgrade()
    {
        if (PlayerPrefs.GetInt("BaseUpgrade", 0) == 1)
        {
            return;
        }

        if (EconomyManager.instance == null)
        {
            return;
        }

        if (EconomyManager.instance.currentMoney >= baseUpgradeCost)
        {
            EconomyManager.instance.DeductMoney(baseUpgradeCost);

            PlayerPrefs.SetInt("BaseUpgrade", 1);
            PlayerPrefs.Save();
            baseUpgradeButton.SetActive(false);
        }
    }

    public void BuySugarUpgrade()
    {
        if (PlayerPrefs.GetInt("SugarUpgrade", 0) == 1)
            return;

        if (EconomyManager.instance == null)
            return;

        if (EconomyManager.instance.currentMoney >= sugarUpgradeCost)
        {
            EconomyManager.instance.DeductMoney(sugarUpgradeCost);

            PlayerPrefs.SetInt("SugarUpgrade", 1);
            PlayerPrefs.Save();

            sugarUpgradeButton.SetActive(false);
        }
    }
}
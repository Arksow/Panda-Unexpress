using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private int baseUpgradeCost = 100;

    [SerializeField] private GameObject baseUpgradeButton;

    public void BuyChocolateUpgrade()
    {
        if (EconomyManager.instance == null)
        {
            return;
        }

        if (EconomyManager.instance.currentMoney >= baseUpgradeCost)
        {
            EconomyManager.instance.DeductMoney(baseUpgradeCost);

            PlayerPrefs.SetInt("ChocolateUpgrade", 1);
            PlayerPrefs.Save();
            baseUpgradeButton.SetActive(false);
            if (EconomyManager.instance != null)
            {
                EconomyManager.instance.DeductMoney(100);
            }
        }
    }
}

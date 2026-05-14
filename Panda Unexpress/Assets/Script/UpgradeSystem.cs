using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    private int baseUpgradeCost = 100;

    [SerializeField] private GameObject baseUpgradeButton;

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
}
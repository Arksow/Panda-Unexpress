using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    private int baseUpgradeCost = 100;
    private int sugarUpgradeCost = 100;

    [SerializeField] private GameObject baseUpgradeButton;
    [SerializeField] private GameObject sugarUpgradeButton;

    [SerializeField] private TextMeshProUGUI notEnoughMoneyText;

    public void BuyBaseUpgrade()
    {
        if (PlayerPrefs.GetInt("BaseUpgrade", 0) == 1)
        {
            baseUpgradeButton.SetActive(false);
            return;
        }

        if (EconomyManager.instance == null)
            return;

        if (EconomyManager.instance.currentMoney >= baseUpgradeCost)
        {
            EconomyManager.instance.DeductMoney(baseUpgradeCost);

            PlayerPrefs.SetInt("BaseUpgrade", 1);
            PlayerPrefs.Save();

            baseUpgradeButton.SetActive(false);
        }
        else
        {
            ShowNotEnoughMoney();
        }
    }

    public void BuySugarUpgrade()
    {
        if (PlayerPrefs.GetInt("SugarUpgrade", 0) == 1)
        {
            sugarUpgradeButton.SetActive(false);
            return;
        }

        if (EconomyManager.instance == null)
            return;

        if (EconomyManager.instance.currentMoney >= sugarUpgradeCost)
        {
            EconomyManager.instance.DeductMoney(sugarUpgradeCost);

            PlayerPrefs.SetInt("SugarUpgrade", 1);
            PlayerPrefs.Save();

            sugarUpgradeButton.SetActive(false);
        }
        else
        {
            ShowNotEnoughMoney();
        }
    }

    void ShowNotEnoughMoney()
    {
        if (notEnoughMoneyText == null) return;

        notEnoughMoneyText.text = "Not enough money!";
        notEnoughMoneyText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideNotEnoughMoney));
        Invoke(nameof(HideNotEnoughMoney), 1.5f);
    }

    void HideNotEnoughMoney()
    {
        if (notEnoughMoneyText == null) return;

        notEnoughMoneyText.gameObject.SetActive(false);
    }
}
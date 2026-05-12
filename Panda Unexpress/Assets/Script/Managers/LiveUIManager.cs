using UnityEngine;
using TMPro;

public class LiveUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI strikeText;
    public TextMeshProUGUI activeCustomersText;

    [Header("Manager References")]
    public GameSystem gameSystem;

    void Update()
    {
        if (EconomyManager.instance != null && moneyText != null)
        {
            moneyText.text = $"Earned: ${EconomyManager.instance.currentMoney}";
        }

        if (gameSystem != null)
        {
            if (waveText != null)
            {
                waveText.text = $"Day: {gameSystem.currentWave}";
            }

            if (strikeText != null)
            {
                string visualStrikes = "";

                for (int i = 0; i < gameSystem.failedOrders; i++)
                {
                    visualStrikes += "<color=red>X</color> ";
                }

                strikeText.text = $"Strikes: {visualStrikes}";
            }

            if (activeCustomersText != null)
            {
                activeCustomersText.text = $"Waiting: {gameSystem.activeCustomers}";
            }
        }
    }
}
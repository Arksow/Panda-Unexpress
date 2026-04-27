using UnityEngine;
using TMPro;

public class GameOverUIManager : MonoBehaviour
{
    public TextMeshProUGUI earningsText;
    public TextMeshProUGUI highscoreText;

    private void Start()
    {
        string currentPlayer = PlayerPrefs.GetString("PlayerName", "Player");

        if (EconomyManager.instance != null)
        {
            int earnings = EconomyManager.instance.currentMoney;
            earningsText.text = $"Earnings: ${earnings}";
            EconomyManager.instance.EndGame(currentPlayer);
            int highscore = PlayerPrefs.GetInt($"{currentPlayer}_Highscore", 0);
            highscoreText.text = $"Highscore: ${highscore}";
        }
    }

    public void OnRetry()
    {
        EconomyManager.instance.currentMoney = 0;
        SceneTransitionManager.instance.StartShift();
    }

    public void OnMainMenu()
    {
        EconomyManager.instance.currentMoney = 0;
        SceneTransitionManager.instance.ReturnToMenu();
    }
}

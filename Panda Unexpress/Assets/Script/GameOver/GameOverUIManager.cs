using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    [Header("UI Canvas")]
    public GameObject gameOverCanvas;

    [Header("UI Elements")]
    public TextMeshProUGUI earningsText;
    public TextMeshProUGUI highscoreText;
    public GameObject newHighscoreAlert;

    [Header("Audio")]
    public AudioClip buttonSound;

    private void Start()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
    }

    public void ShowGameOverUI()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        string currentPlayer = PlayerPrefs.GetString(SaveKeys.CURRENT_PLAYER, "Player");
        int oldHighscore = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(currentPlayer), 0);

        if (EconomyManager.instance != null)
        {
            int earnings = EconomyManager.instance.currentMoney;
            earningsText.text = $"Earnings: ${earnings}";

            EconomyManager.instance.EndGame(currentPlayer);

            int updatedHighscore = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(currentPlayer), 0);
            highscoreText.text = $"Highscore: ${updatedHighscore}";

            if (earnings > oldHighscore && newHighscoreAlert != null)
            {
                newHighscoreAlert.SetActive(true);
            }
            else if (newHighscoreAlert != null)
            {
                newHighscoreAlert.SetActive(false);
            }
        }
    }

    public void OnRetry()
    {
        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlayGlobalSFX(buttonSound);
        }

        if (EconomyManager.instance != null)
        {
            EconomyManager.instance.currentMoney = 0;
        }

        if (SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.StartShift();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnMainMenu()
    {
        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlayGlobalSFX(buttonSound);
        }

        if (EconomyManager.instance != null)
        {
            EconomyManager.instance.currentMoney = 0;
        }

        if (SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.ReturnToMenu();
        }
    }
}
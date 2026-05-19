using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverUIManager : MonoBehaviour
{
    [Header("UI Canvas")]
    public GameObject gameOverCanvas;

    [Header("UI Elements")]
    public TextMeshProUGUI earningsText;
    public TextMeshProUGUI highscoreText;
    public GameObject newHighscoreAlert;

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

            EconomyManager.instance.EndGame();

            if (earnings > oldHighscore && earnings > 0)
            {
                highscoreText.text = $"Highscore: ${earnings}";
                if (newHighscoreAlert != null) newHighscoreAlert.SetActive(true);
            }
            else
            {
                highscoreText.text = $"Highscore: ${oldHighscore}";
                if (newHighscoreAlert != null) newHighscoreAlert.SetActive(false);
            }
        }
    }

    public void OnRetry()
    {
        if (EconomyManager.instance != null)
        {
            EconomyManager.instance.currentMoney = 0;
            PlayerPrefs.SetInt("BaseUpgrade", 0);
            PlayerPrefs.SetInt("SugarUpgrade", 0);
            PlayerPrefs.Save();
        }

        if (SceneTransitionManager.instance != null)
            SceneTransitionManager.instance.StartShift();
        else
            StartCoroutine(LoadAsync(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;
    }

    public void OnMainMenu()
    {
        if (EconomyManager.instance != null)
        {
            EconomyManager.instance.currentMoney = 0;
            PlayerPrefs.SetInt("BaseUpgrade", 0);
            PlayerPrefs.SetInt("SugarUpgrade", 0);
            PlayerPrefs.Save();
        }

        if (SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.ReturnToMenu();
        }
    }
}
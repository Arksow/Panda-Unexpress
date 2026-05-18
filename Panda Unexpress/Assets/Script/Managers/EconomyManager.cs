using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;

    public int currentMoney = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentMoney = 0;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Added " + amount + " money. Current total: " + currentMoney);
    }

    public void DeductMoney(int amount)
    {
        currentMoney -= amount;
        if (currentMoney < 0) currentMoney = 0;
    }

    public void EndGame()
    {
        string currentPlayer = PlayerPrefs.GetString(SaveKeys.CURRENT_PLAYER, "Guest_Panda");
        int prevHighscore = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(currentPlayer), 0);

        if (currentMoney > prevHighscore)
        {
            PlayerPrefs.SetInt(SaveKeys.GetHighScoreKey(currentPlayer), currentMoney);
            Debug.Log($"New Highscore for {currentPlayer}: ${currentMoney}");
        }

        int globalHighScore = PlayerPrefs.GetInt("Global_HighScore_Value", 0);
        if (currentMoney > globalHighScore)
        {
            PlayerPrefs.SetInt("Global_HighScore_Value", currentMoney);
            PlayerPrefs.SetString("Global_HighScore_Name", currentPlayer);
            Debug.Log($"New GLOBAL Highscore! {currentPlayer}: ${currentMoney}");
        }

        int totalMoney = PlayerPrefs.GetInt(SaveKeys.GetTotalMoneyKey(currentPlayer), 0);
        PlayerPrefs.SetInt(SaveKeys.GetTotalMoneyKey(currentPlayer), totalMoney + currentMoney);
        PlayerPrefs.Save();
    }
}
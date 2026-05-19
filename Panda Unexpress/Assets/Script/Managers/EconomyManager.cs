using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;

    public int currentMoney = 0;

    private bool hasSavedThisRun = false;

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
        hasSavedThisRun = false;
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
        if (hasSavedThisRun)
        {
            Debug.Log("Game already saved this run! Preventing double-entry.");
            return;
        }

        hasSavedThisRun = true;

        string currentPlayer = PlayerPrefs.GetString(SaveKeys.CURRENT_PLAYER, "Guest_Panda");
        int prevHighscore = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(currentPlayer), 0);

        if (currentMoney > prevHighscore)
        {
            PlayerPrefs.SetInt(SaveKeys.GetHighScoreKey(currentPlayer), currentMoney);
            Debug.Log($"New Highscore for {currentPlayer}: ${currentMoney}");
        }

        for (int i = 0; i < 5; i++)
        {
            int currentRankScore = PlayerPrefs.GetInt("Global_Score_" + i, 0);

            if (currentMoney > currentRankScore)
            {
                for (int j = 4; j > i; j--)
                {
                    PlayerPrefs.SetInt("Global_Score_" + j, PlayerPrefs.GetInt("Global_Score_" + (j - 1), 0));
                    PlayerPrefs.SetString("Global_Name_" + j, PlayerPrefs.GetString("Global_Name_" + (j - 1), "---"));
                }

                PlayerPrefs.SetInt("Global_Score_" + i, currentMoney);
                PlayerPrefs.SetString("Global_Name_" + i, currentPlayer);
                Debug.Log($"New GLOBAL Highscore! {currentPlayer}: ${currentMoney} at Rank {i + 1}");
                break;
            }
        }

        int totalMoney = PlayerPrefs.GetInt(SaveKeys.GetTotalMoneyKey(currentPlayer), 0);
        PlayerPrefs.SetInt(SaveKeys.GetTotalMoneyKey(currentPlayer), totalMoney + currentMoney);
        PlayerPrefs.Save();
    }
}
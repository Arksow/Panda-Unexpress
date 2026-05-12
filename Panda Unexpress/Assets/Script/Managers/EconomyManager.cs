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

    public void EndGame(string playerName)
    {
        float prevHighscore = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(playerName), 0);
        if (prevHighscore < currentMoney)
        {
            PlayerPrefs.SetInt(SaveKeys.GetHighScoreKey(playerName), currentMoney);
        }

        int totalMoney = PlayerPrefs.GetInt(SaveKeys.GetTotalMoneyKey(playerName), 0);
        PlayerPrefs.SetInt(SaveKeys.GetTotalMoneyKey(playerName), totalMoney + currentMoney);
        PlayerPrefs.Save();
    }
}
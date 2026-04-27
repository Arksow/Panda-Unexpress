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

    public void EndGame(string playerName)
    {
        float prevHighscore = PlayerPrefs.GetInt(playerName + "_HighScore", 0);
        if (prevHighscore < currentMoney)
        {
            PlayerPrefs.SetInt(playerName + "_HighScore", currentMoney);
        }

        int totalMoney = PlayerPrefs.GetInt(playerName + "_TotalMoney", 0) + currentMoney;
        PlayerPrefs.SetInt(playerName + "_TotalMoney", totalMoney + currentMoney);
        PlayerPrefs.Save();
    }
}
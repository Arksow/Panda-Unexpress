using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public string playerName;
    public int highScore;

    public void LoadPlayerProfile(string playerName)
    {
        highScore = PlayerPrefs.GetInt(playerName + "_HighScore", 0);
        PlayerPrefs.SetString("CurrentPlayer", playerName);
        PlayerPrefs.Save();
    }
}
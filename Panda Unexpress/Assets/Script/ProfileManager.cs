using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public string playerName;
    public int highScore;

    private void LoadPlayerProfile(string playerName)
    {
        highScore = PlayerPrefs.GetInt(playerName + "_HighScore", 0);
        PlayerPrefs.SetString("CurrentPlayer", playerName);
        PlayerPrefs.Save();
    }
}
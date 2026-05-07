using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public string playerName;
    public int highScore;

    public void LoadPlayerProfile(string playerName)
    {
        highScore = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(playerName), 0);
        PlayerPrefs.SetString(SaveKeys.CURRENT_PLAYER, playerName);
        PlayerPrefs.Save();
    }
}
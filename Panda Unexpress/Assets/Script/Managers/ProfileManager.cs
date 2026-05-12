using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public string playerName;
    public int highScore;

    public void LoadPlayerProfile(string inputName)
    {
        string sanitizedName = inputName.Trim();
        if (string.IsNullOrEmpty(sanitizedName))
        {
            sanitizedName = "Guest_Panda";
        }

        playerName = sanitizedName;
        highScore = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(playerName), 0);
        PlayerPrefs.SetString(SaveKeys.CURRENT_PLAYER, playerName);
        PlayerPrefs.Save();
    }
}
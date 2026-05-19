using UnityEngine;
using TMPro;

public class UserManager : MonoBehaviour
{
    [Tooltip("Drag the 3 individual TextMeshPro objects here for Left, Middle, and Right letters.")]
    public TextMeshProUGUI[] letterDisplays = new TextMeshProUGUI[3];
    public TextMeshProUGUI highScoreDisplay;

    public Color activeColor = Color.yellow;
    public Color inactiveColor = Color.white;
    public ProfileManager profileManager;

    private char[] currentLetters = new char[3] { 'A', 'A', 'A' };

    private int activeLetterIndex = 0;

    private void Start()
    {
        string lastPlayer = PlayerPrefs.GetString(SaveKeys.CURRENT_PLAYER, "AAA");

        if (lastPlayer.Length >= 3)
        {
            currentLetters[0] = lastPlayer[0];
            currentLetters[1] = lastPlayer[1];
            currentLetters[2] = lastPlayer[2];
        }

        if (highScoreDisplay != null)
        {
            string leaderboardText = "TOP 5 ALL-TIME BEST\n";
            for (int i = 0; i < 5; i++)
            {
                string rankName = PlayerPrefs.GetString("Global_Name_" + i, "---");
                int rankScore = PlayerPrefs.GetInt("Global_Score_" + i, 0);
                leaderboardText += $"{i + 1}. {rankName} - ${rankScore}\n";
            }
            highScoreDisplay.text = leaderboardText;
        }

        UpdateDisplays();

        if (profileManager != null)
            profileManager.LoadPlayerProfile(new string(currentLetters));
    }

    public void CycleLetterUp()
    {
        char currentChar = currentLetters[activeLetterIndex];

        if (currentChar == 'Z')
            currentChar = 'A';
        else
            currentChar++;

        currentLetters[activeLetterIndex] = currentChar;

        UpdateDisplays();
    }

    public void CycleLetterDown()
    {
        char currentChar = currentLetters[activeLetterIndex];

        if (currentChar == 'A')
            currentChar = 'Z';
        else
            currentChar--;

        currentLetters[activeLetterIndex] = currentChar;

        UpdateDisplays();
    }

    public void ConfirmSelection()
    {
        activeLetterIndex++;

        if (activeLetterIndex > 2)
        {
            activeLetterIndex = 0;

            if (profileManager != null)
            {
                string tempName = new string(currentLetters);
                profileManager.LoadPlayerProfile(tempName);
            }
        }

        UpdateDisplays();
    }

    public void StartGame()
    {
        if (profileManager != null)
        {
            string finalName = new string(currentLetters);
            profileManager.LoadPlayerProfile(finalName);
            Debug.Log("Locked in player: " + finalName);
        }

        if (SceneTransitionManager.instance != null)
        {
            SceneTransitionManager.instance.StartShift();
        }
        else
        {
            Debug.LogError("Panda Unexpress: SceneTransitionManager is missing!");
        }
    }

    private void UpdateDisplays()
    {
        for (int i = 0; i < 3; i++)
        {
            if (letterDisplays[i] != null)
            {
                letterDisplays[i].text = currentLetters[i].ToString();

                if (i == activeLetterIndex)
                    letterDisplays[i].color = activeColor;
                else
                    letterDisplays[i].color = inactiveColor;
            }
        }
    }
}
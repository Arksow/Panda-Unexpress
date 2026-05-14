using UnityEngine;
using TMPro;

public class ArcadeLoginManager : MonoBehaviour
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
        UpdateDisplays();
    }

    public void CycleUp()
    {
        if (currentLetters[activeLetterIndex] == 'Z')
            currentLetters[activeLetterIndex] = 'A';
        else
            currentLetters[activeLetterIndex]++;

        UpdateDisplays();
    }

    public void CycleDown()
    {
        if (currentLetters[activeLetterIndex] == 'A')
            currentLetters[activeLetterIndex] = 'Z';
        else
            currentLetters[activeLetterIndex]--;

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

        if (profileManager != null && highScoreDisplay != null)
        {
            string tempName = new string(currentLetters);
            int score = PlayerPrefs.GetInt(SaveKeys.GetHighScoreKey(tempName), 0);
            highScoreDisplay.text = $"High Score:\n${score}";
        }
    }
}
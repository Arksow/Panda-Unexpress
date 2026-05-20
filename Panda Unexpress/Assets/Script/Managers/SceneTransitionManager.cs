using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for UI elements
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public AudioClip bgm;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string gameScene = "MainGame";
    public string gameOverScene = "GameOver";
    public string tutorialScene = "TurtorialScene";

    [Header("Loading Screen UI")]
    public GameObject loadingScreenPanel;
    public Image loadingImageTarget;
    public Sprite loadingSprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartShift()
    {
        StartCoroutine(LoadSceneAsync(gameScene));
    }

    public void StartTutorial()
    {
        StartCoroutine(LoadSceneAsync(tutorialScene));
    }

    public void EndShift()
    {
        StartCoroutine(LoadSceneAsync(gameOverScene));
    }

    public void ReturnToMenu()
    {
        StartCoroutine(LoadSceneAsync(mainMenuScene));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingScreenPanel != null)
        {
            if (loadingImageTarget != null && loadingSprite != null)
            {
                loadingImageTarget.sprite = loadingSprite;
            }

            loadingScreenPanel.SetActive(true);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (loadingScreenPanel != null)
        {
            loadingScreenPanel.SetActive(false);
        }

        if (AudioController.Instance != null)
        {
            AudioController.Instance.PlayMusic(bgm);
        }

        Debug.Log("Successfully transitioned to: " + sceneName);
    }
}
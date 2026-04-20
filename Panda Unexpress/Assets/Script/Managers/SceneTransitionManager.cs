using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string gameScene = "MainGame";
    public string gameOverScene = "GameOver";

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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Successfully transitioned to: " + sceneName);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneResetter : MonoBehaviour
{
    public string tutorialScene = "TurtorialScene";
    public string mainMenuScene = "MainMenu";

    public void RestartTutorial()
    {
        SceneManager.LoadScene(tutorialScene);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    private IEnumerator LoadScene(string sceneName)
    {

        Time.timeScale = 1f;

        
        if (AudioController.Instance != null)
        {
            AudioController.Instance.StopAllCoroutines();
        }

        //reset
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = true;

        while (!op.isDone)
        {
            yield return null;
        }

       
    }
}
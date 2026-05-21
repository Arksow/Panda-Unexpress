using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject Settings;
    public GameObject Level;
    public GameObject Controls;

    public  GameObject[] ObjectsToHide;

    public AudioClip menuBGM;
    public void HideObjects()
    {
        foreach (GameObject obj in ObjectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
    public void ShowObjects()
    {
        foreach (GameObject obj in ObjectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
    public void Start()
    {
        menu.SetActive(true);
        AudioController.Instance.PlayMusic(menuBGM, 0.5f);
    }

    public void OnClickPlay()
    {
        menu.SetActive(false);
        Level.SetActive(true);
        HideObjects();

        AudioController.Instance.PlayMusic(menuBGM, 0.5f);
    }
    public void OnClickBack()
    {
        menu.SetActive(true);
        Level.SetActive(false);
        ShowObjects();
    }
    public void OnClickSettings()
    {
        menu.SetActive(false);
        Settings.SetActive(true);
        HideObjects();
    }

    public void OnClickSettingsBack()
    {
        menu.SetActive(true);
        Settings.SetActive(false);
        ShowObjects();
    }
    public void OnClickControls()
    {
        Settings.SetActive(false);
        Controls.SetActive(true);
    }

    public void OnClickControlsBack()
    {
        Settings.SetActive(true);
        Controls.SetActive(false);
    }
    public void Quit()
    {

        #if UNITY_EDITOR
              UnityEditor.EditorApplication.isPlaying = false;
        #else
              Application.Quit();
        #endif

    }
}

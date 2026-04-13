using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject Settings;
    public GameObject Level;
    public GameObject Controls;


    public void Start()
    {
        menu.SetActive(true);
        //in case add more
    }

    public void OnClickPlay()
    {
        menu.SetActive(false);
        Level.SetActive(true);
    }
    public void OnClickBack()
    {
        menu.SetActive(true);
        Level.SetActive(false);
    }
    public void OnClickSettings()
    {
        menu.SetActive(false);
        Settings.SetActive(true);
    }

    public void OnClickSettingsBack()
    {
        menu.SetActive(true);
        Settings.SetActive(false);
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

using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    public Scrollbar musicSlider;
    public Scrollbar sfxSlider;
    public Scrollbar MasterVolSlider;
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        MasterVolSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        // Add listeners
        musicSlider.onValueChanged.AddListener(ChangeVolume);
        sfxSlider.onValueChanged.AddListener(SFXChangeVolume);
        MasterVolSlider.onValueChanged.AddListener(MasterChangeVolume);
    }

    private void MasterChangeVolume(float value)
    {
        AudioController.Instance.SetMasterVolume(value);
        //PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void ChangeVolume(float value)
    {
        AudioController.Instance.SetVolume(value);
    }

    public void SFXChangeVolume(float value)
    {
        AudioController.Instance.SetVolumeOfSfx(value);
    }
    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey("MusicVolume");
        PlayerPrefs.DeleteKey("SFXVolume");
        PlayerPrefs.DeleteKey("MasterVolume");

        float defaultMusicVol = 1f;
        float defaultSFXVol = 1f;
        float defaultMasterVol = 1f;
        AudioController.Instance.SetVolume(defaultMusicVol);
        AudioController.Instance.SetVolumeOfSfx(defaultSFXVol);
        AudioController.Instance.SetMasterVolume(defaultMasterVol);

        MasterVolSlider.value = defaultMasterVol;
        musicSlider.value = defaultMusicVol;
         sfxSlider.value = defaultSFXVol;
    }


    //in case test
   
}

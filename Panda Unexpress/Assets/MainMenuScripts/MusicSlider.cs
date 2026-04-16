using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MusicSlider : MonoBehaviour
{
    public Scrollbar musicSlider;
    public Scrollbar sfxSlider;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // Add listeners
        musicSlider.onValueChanged.AddListener(ChangeVolume);
        sfxSlider.onValueChanged.AddListener(SFXChangeVolume);

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

        float defaultMusicVol = 1f;
        float defaultSFXVol = 1f;

        AudioController.Instance.SetVolume(defaultMusicVol);
        AudioController.Instance.SetVolumeOfSfx(defaultSFXVol);

         musicSlider.value = defaultMusicVol;
         sfxSlider.value = defaultSFXVol;
    }
}

using UnityEngine;
using UnityEngine.UI;

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
        AudioController.Instance.SetVolume(value);
    }
}

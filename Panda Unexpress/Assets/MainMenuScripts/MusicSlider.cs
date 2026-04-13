using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    public Scrollbar slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 1f); //slider to control music globally
        slider.onValueChanged.AddListener(ChangeVolume);

    }

    void ChangeVolume(float value)
    {
        AudioController.Instance.SetVolume(value);
    }
}

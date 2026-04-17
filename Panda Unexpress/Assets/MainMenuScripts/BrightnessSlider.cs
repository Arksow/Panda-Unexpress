using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class BrightnessSlider : MonoBehaviour
{
    public Scrollbar BrightnessSlide;
    public Image Brightness;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat("Brightness", 0f);
        BrightnessSlide.value = saved;
        SetBrightness(saved);

        BrightnessSlide.onValueChanged.AddListener(SetBrightness);
    }

    public void SetBrightness(float value)
    {
        Color c = Brightness.color;
        c.a = value;   //image set brightness overlay
        Brightness.color = c;

        PlayerPrefs.SetFloat("Brightness", value);
    }
    public void ResetSettingForBrightness()
    {
        float defaultValue = 0f;
        PlayerPrefs.SetFloat("Brightness", defaultValue);

        Color c = Brightness.color;
        c.a = defaultValue;   //image set brightness overlay
        BrightnessSlide.value = defaultValue;
        Brightness.color = c;
    }
}


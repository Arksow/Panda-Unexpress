using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class BrightnessSlider : MonoBehaviour //changed the whole thing cause thing is broken and i can't fix it
{
    //public Scrollbar BrightnessSlide;
    //public Image Brightness;
    //public static BrightnessSlider Instance;

    //void Start()
    //{
    //    float saved = PlayerPrefs.GetFloat("Brightness", 0f);
    //    BrightnessSlide.value = saved;
    //    SetBrightness(saved);

    //    BrightnessSlide.onValueChanged.AddListener(SetBrightness);
    //}
    //void Awake() //just to test
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    //public void SetBrightness(float value)
    //{
    //    Color c = Brightness.color;
    //    c.a = value;   //image set brightness overlay
    //    Brightness.color = c;

    //    PlayerPrefs.SetFloat("Brightness", value);
    //}
    //public void ResetSettingForBrightness()
    //{
    //    float defaultValue = 0f;
    //    PlayerPrefs.SetFloat("Brightness", defaultValue);

    //    Color c = Brightness.color;
    //    c.a = defaultValue;   //image set brightness overlay
    //    BrightnessSlide.value = defaultValue;
    //    Brightness.color = c;
    //}
    private BrightnessSlider instance;
    private Canvas canvas;
    private Image blackImage;
    private Transform target; //player

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        CreateCanvas();
    }

    void Start()
    {
        SetBrightness(PlayerPrefs.GetFloat("Brightness", 0f));
    }

    void LateUpdate()
    {
        if (target == null)
        {
            if (Camera.main != null)
                target = Camera.main.transform;
            else
                return;
        }

        // follow player/camera
        canvas.transform.position = target.position + target.forward * 0.5f;
        canvas.transform.rotation = target.rotation;
    }

    void CreateCanvas()
    {
        GameObject go = new GameObject("BrightnessCanvas");
        canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;

        go.AddComponent<CanvasScaler>();

        canvas.transform.localScale = Vector3.one * 0.001f;

        GameObject img = new GameObject("BlackImage");
        img.transform.SetParent(canvas.transform, false);

        blackImage = img.AddComponent<Image>();
        blackImage.color = new Color(0, 0, 0, 0);

        RectTransform rt = img.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(2000, 2000);
    }

    public void SetBrightness(float value)
    {
        if (blackImage == null) return;

        Color c = blackImage.color;
        c.a = value;
        blackImage.color = c;

        PlayerPrefs.SetFloat("Brightness", value);
    }
}


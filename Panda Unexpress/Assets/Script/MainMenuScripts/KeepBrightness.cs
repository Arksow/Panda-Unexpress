using UnityEngine;

public class KeepBrightness : MonoBehaviour
{
    public static KeepBrightness Instance;

    void Awake() //just to test
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

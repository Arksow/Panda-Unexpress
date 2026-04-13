using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource musicSource;

    private void Awake()
    {
        //global //singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load saved volume
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        Debug.Log("Volume set to: " + volume);
    }
}

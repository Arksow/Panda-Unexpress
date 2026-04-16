using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public Sound[] music; //put under audio list
    public Sound[] sfx;

    public string CurrentClip = "";
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
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 50f);
        SetVolume(savedVolume);

      


        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 50f); //forever
        SetVolume(savedSfxVolume);
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        Debug.Log("Volume set to: " + volume);
    }

    public void SetVolumeOfSfx(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        Debug.Log("Volume set to: " + volume);
    }

    public void PlayMusic(string trackName) //create empty gameobject with audio source and SceneMusic Scrip
    {
        if (CurrentClip == trackName)
            return;

        foreach (Sound s in music)
        {
            if (s.name == trackName)
            {
                musicSource.clip = s.clip;
                musicSource.loop = true;
                musicSource.Play();
                CurrentClip = trackName;

                Debug.Log("Playing Music: " + trackName);
                return;
            }
        }

        Debug.LogWarning("Music not found: " + trackName);
    }

   //play sfx
    public void PlaySFX(string soundName) //just call into whatever button or what
    {
        foreach (Sound s in sfx)
        {
            if (s.name == soundName)
            {
                sfxSource.PlayOneShot(s.clip);
                return;
            }
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    float masterVolume = 1f;
    float musicVolume = 1f;
    float sfxVolume = 1f;
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

        //// Load saved volume
        //float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 50f);
        //SetVolume(savedVolume);




        //float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 50f); //forever
        //SetVolume(savedSfxVolume);
        //get from music slider instead of old way
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyVolumes();
    }
    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        SetupAudioSources();
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    LoadVolumes();
    //}

    //void SetupAudioSources()
    //{

    //    transform.SetParent(null);


    //    AudioListener listener = GetComponent<AudioListener>();
    //    if (listener != null)
    //    {
    //        Destroy(listener);
    //    }

    //    //create automatically
    //    if (musicSource == null)
    //    {
    //        musicSource = gameObject.AddComponent<AudioSource>();
    //        musicSource.loop = true;
    //        musicSource.playOnAwake = false;
    //    }

    //    if (sfxSource == null)
    //    {
    //        sfxSource = gameObject.AddComponent<AudioSource>();
    //        sfxSource.loop = false;
    //        sfxSource.playOnAwake = false;
    //    }
    //}

    //void LoadVolumes()
    //{
    //    float musicVol = PlayerPrefs.GetFloat("MusicVolume", 30f);
    //    float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 30f);

    //    musicSource.volume = musicVol;
    //    sfxSource.volume = sfxVol;
    //}
    //public void SetVolume(float volume)
    //{
    //    musicSource.volume = volume;
    //    PlayerPrefs.SetFloat("MusicVolume", volume);
    //    Debug.Log("Volume set to: " + volume);
    //}

    //public void SetVolumeOfSfx(float volume)
    //{
    //    sfxSource.volume = volume;
    //    PlayerPrefs.SetFloat("SFXVolume", volume);
    //    Debug.Log("Volume set to: " + volume);
    //}
    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        ApplyVolumes();
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetVolume(float value)
    {
        musicVolume = value;
        ApplyVolumes();
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetVolumeOfSfx(float value)
    {
        sfxVolume = value;
        ApplyVolumes();
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void ApplyVolumes()
    {
        musicSource.volume = masterVolume * musicVolume;
        sfxSource.volume = masterVolume * sfxVolume; //general
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

using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource musicSource;
    public AudioSource staticSource;

    public string CurrentClip = "";
    float masterVolume = 1f;
    float musicVolume = 1f;
    float staticVolume = 1f;

    private void Awake()
    {
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

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        staticVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        ApplyVolumes();
    }
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
        staticVolume = value;
        ApplyVolumes();
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void ApplyVolumes()
    {
        musicSource.volume = masterVolume * musicVolume;
        staticSource.volume = masterVolume * staticVolume;
    }

    public void PlayGlobalSFX(AudioClip clip)
    {
        if (clip != null)
        {
            staticSource.PlayOneShot(clip);
        }
    }

    public void PlaySpatialSFX(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = position;

            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = masterVolume * staticVolume;
            audioSource.spatialBlend = 1.0f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 10f;

            audioSource.Play();
            Destroy(tempAudio, clip.length);
        }
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip)
            return;

        musicSource.clip = newClip;
        musicSource.Play();
    }
}
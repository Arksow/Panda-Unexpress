using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource staticSource;

    public AudioClip buttonClickSound;

    public string CurrentClip = "";
    float masterVolume = 1f;
    float musicVolume = 1f;
    float staticVolume = 1f;

    private float currentMusicMultiplier = 1f;

    public bool IsPlaying() => musicSource != null && musicSource.isPlaying;

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
        musicSource.volume = masterVolume * musicVolume * currentMusicMultiplier;
        staticSource.volume = masterVolume * staticVolume;
    }

    public void PlayGlobalSFX(AudioClip clip, float volumeBoost = 1f)
    {
        if (this == null || staticSource == null) return;
        if (clip != null)
        {
            if (staticSource != null)
            {
                staticSource.PlayOneShot(clip, volumeBoost);
            }
            else
            {
                Debug.LogWarning("AudioController: staticSource is missing or was destroyed! Please attach an AudioSource directly to the AudioController GameObject.");
            }
        }
    }

    public void PlaySpatialSFX(AudioClip clip, Vector3 position)
    {
        if (this == null) return;

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

    public void PlayClick()
    {
        if (buttonClickSound != null)
        {
            PlayGlobalSFX(buttonClickSound, 2.5f);
        }
    }

    public void PlayMusic(AudioClip newClip, float trackVolume = 1f)
    {
        if (musicSource.clip == newClip && musicSource.isPlaying)
            return;

        currentMusicMultiplier = trackVolume;

        musicSource.clip = newClip;
        musicSource.loop = true;
        musicSource.Play();

        ApplyVolumes();
    }
}
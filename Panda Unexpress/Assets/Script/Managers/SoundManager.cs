using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource backgroundSource;
    public AudioSource staticSource;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
            audioSource.spatialBlend = 1.0f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 10f;

            audioSource.Play();
            Destroy(tempAudio, clip.length);
        }
    }
}

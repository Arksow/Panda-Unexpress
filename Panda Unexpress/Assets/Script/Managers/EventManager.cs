using UnityEngine;
using System.Collections;

public enum Events { None, SugarSpoil, HotWeather, ThirstyPlayer }

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public Events currentEvent = Events.None;
    public bool isHotWeather = false;

    public float thirstTimer = 30f;
    private Coroutine thirstCoroutine;
    private Coroutine coughCoroutine;
    public GameSystem gameSystem;

    [Header("Audio Tracks")]
    public AudioClip summerSoundtrack;
    public AudioClip defaultSoundtrack;

    [Header("Player Status SFX")]
    public AudioSource breathingSource;
    public AudioClip heavyBreathingSound;

    [Header("Thirst Event Feedback")]
    public AudioSource playerVoiceSource;
    public AudioClip coughSound;
    public float minCoughInterval = 4f;
    public float maxCoughInterval = 8f;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.5f)
            {
                if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
                    TriggerSpecificEvent(Events.SugarSpoil);

                if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
                    TriggerSpecificEvent(Events.HotWeather);

                if (OVRInput.GetDown(OVRInput.Button.Three, OVRInput.Controller.LTouch))
                    TriggerSpecificEvent(Events.ThirstyPlayer);
            }
        }
    }

    public void RollForRandomEvent()
    {
        if (currentEvent != Events.None) return;
        int diceRoll = Random.Range(0, 100);

        if (diceRoll < 20)
        {
            Debug.Log("Dice Roll: 25% Chance Hit! Triggering Random Event.");

            Events[] possibleEvents = { Events.SugarSpoil, Events.HotWeather, Events.ThirstyPlayer };
            Events chosenEvent = possibleEvents[Random.Range(0, possibleEvents.Length)];

            TriggerSpecificEvent(chosenEvent);
        }
        else
        {
            Debug.Log("Dice Roll: Event avoided this time.");
        }
    }

    public void TriggerSpecificEvent(Events eventToTrigger)
    {
        currentEvent = eventToTrigger;

        switch (currentEvent)
        {
            case Events.SugarSpoil:
                Debug.Log("Event Triggered: Sugar Machine Broken!");
                break;
            case Events.HotWeather:
                Debug.Log("Event Triggered: Hot Weather! Ice melts faster.");
                isHotWeather = true;

                if (AudioController.Instance != null && summerSoundtrack != null)
                {
                    AudioController.Instance.PlayMusic(summerSoundtrack);
                }

                if (breathingSource != null && heavyBreathingSound != null)
                {
                    breathingSource.clip = heavyBreathingSound;
                    breathingSource.loop = true;
                    breathingSource.Play();
                }
                break;
            case Events.ThirstyPlayer:
                thirstCoroutine = StartCoroutine(ThirstCountdown());
                coughCoroutine = StartCoroutine(CoughRoutine());
                break;
        }
    }

    public void ResolveEvent()
    {
        Debug.Log($"Event {currentEvent} Resolved!");

        if (currentEvent == Events.HotWeather)
        {
            if (AudioController.Instance != null && defaultSoundtrack != null)
            {
                AudioController.Instance.PlayMusic(defaultSoundtrack);
            }

            if (breathingSource != null && breathingSource.isPlaying)
            {
                breathingSource.Stop();
            }
        }

        currentEvent = Events.None;
        isHotWeather = false;

        if (thirstCoroutine != null) StopCoroutine(thirstCoroutine);
        if (coughCoroutine != null) StopCoroutine(coughCoroutine);
    }

    private IEnumerator ThirstCountdown()
    {
        Debug.Log("Warning: Make yourself a Boba in 30 seconds!");

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
            OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.LTouch);
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
            OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);

        yield return new WaitForSeconds(0.5f);

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

        yield return new WaitForSeconds(thirstTimer - 0.5f);

        if (currentEvent == Events.ThirstyPlayer)
        {
            Debug.Log("You failed to drink the Boba in time! Taking a strike.");
            if (gameSystem != null)
            {
                gameSystem.AddStrike();
            }
            ResolveEvent();
        }
    }

    private IEnumerator CoughRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (currentEvent == Events.ThirstyPlayer)
        {
            if (playerVoiceSource != null && coughSound != null)
            {
                playerVoiceSource.PlayOneShot(coughSound);
            }

            float waitTime = Random.Range(minCoughInterval, maxCoughInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
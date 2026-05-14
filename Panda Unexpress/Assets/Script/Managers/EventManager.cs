using UnityEngine;
using System.Collections;

public enum Events { None, SugarSpoil, HotWeather, ThirstyPlayer }

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public int consecutiveSuccesses = 0;
    public int successesToTriggerEvent = 3;

    public Events currentEvent = Events.None;
    public bool isHotWeather = false;

    public float thirstTimer = 30f;
    private Coroutine thirstCoroutine;
    public GameSystem gameSystem;

    [Header("Audio Tracks")]
    public AudioClip summerSoundtrack;
    public AudioClip defaultSoundtrack;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                // 'A' Button on Right Controller
                if (OVRInput.GetDown(OVRInput.Button.One)) TriggerSpecificEvent(Events.SugarSpoil);

                // 'B' Button on Right Controller
                if (OVRInput.GetDown(OVRInput.Button.Two)) TriggerSpecificEvent(Events.HotWeather);

                // 'X' Button on Left Controller
                if (OVRInput.GetDown(OVRInput.Button.Three)) TriggerSpecificEvent(Events.ThirstyPlayer);

                // 'Y' Button on Left Controller
                if (OVRInput.GetDown(OVRInput.Button.Four)) ResolveCurrentEvent();
            }
        }
    }

    public void RegisterSuccess()
    {
        consecutiveSuccesses++;
        if (consecutiveSuccesses >= successesToTriggerEvent && currentEvent == Events.None)
        {
            TriggerRandomEvent();
            consecutiveSuccesses = 0;
        }
    }

    public void RegisterFailure()
    {
        consecutiveSuccesses = 0;
    }

    public void TriggerSpecificEvent(Events eventToTrigger)
    {
        if (currentEvent != Events.None)
        {
            Debug.Log($"Ignored {eventToTrigger}: Another event ({currentEvent}) is already active.");
            return;
        }

        currentEvent = eventToTrigger;
        Debug.Log("EVENT FORCED: " + currentEvent);
        ApplyEventEffects();
    }

    private void TriggerRandomEvent()
    {
        currentEvent = (Events)Random.Range(1, 4);
        Debug.Log("Random event triggered: " + currentEvent);
        ApplyEventEffects();
    }

    private void ApplyEventEffects()
    {
        switch (currentEvent)
        {
            case Events.HotWeather:
                isHotWeather = true;
                if (AudioController.Instance != null && summerSoundtrack != null)
                {
                    AudioController.Instance.PlayMusic(summerSoundtrack);
                }
                break;
            case Events.ThirstyPlayer:
                thirstCoroutine = StartCoroutine(ThirstCountdown());
                break;
            case Events.SugarSpoil:
                break;
        }
    }

    public void ResolveCurrentEvent()
    {
        Debug.Log("Player fixed: " + currentEvent);

        if (currentEvent == Events.HotWeather && AudioController.Instance != null && defaultSoundtrack != null)
        {
            AudioController.Instance.PlayMusic(defaultSoundtrack);
        }

        currentEvent = Events.None;
        isHotWeather = false;

        if (thirstCoroutine != null)
        {
            StopCoroutine(thirstCoroutine);
        }
    }

    private IEnumerator ThirstCountdown()
    {
        Debug.Log("Warning: Make yourself a Boba in 30 seconds!");

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) && OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
        }

        yield return new WaitForSeconds(0.5f);

        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTouch) && OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }

        yield return new WaitForSeconds(thirstTimer - 0.5f);

        if (currentEvent == Events.ThirstyPlayer)
        {
            Debug.Log("Player forgot to drink! STRIKE!");
            gameSystem?.RegisterFailedOrder();
            ResolveCurrentEvent();
        }
    }
}
using UnityEngine;
using System.Collections;

public enum Events { None, SugarSpoil, IceSpoil, HotWeather, LeakyScoop, ThirstyPlayer }

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

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
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

    private void TriggerRandomEvent()
    {
        currentEvent = (Events)Random.Range(1, 6);
        Debug.Log("Event triggered: " + currentEvent);

        switch (currentEvent)
        {
            case Events.HotWeather:
                isHotWeather = true;
                break;
            case Events.ThirstyPlayer:
                thirstCoroutine = StartCoroutine(ThirstCountdown());
                break;
        }
    }

    public void ResolveCurrentEvent()
    {
        Debug.Log("Player fixed" + currentEvent);
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

        yield return new WaitForSeconds(thirstTimer);

        if (currentEvent == Events.ThirstyPlayer)
        {
            Debug.Log("Player forgot to drink! STRIKE!");
            gameSystem?.RegisterFailedOrder();
            ResolveCurrentEvent();
        }
    }
}
using UnityEngine;
using Oculus.Voice;

public class VoiceCommandManager : MonoBehaviour
{
    public static VoiceCommandManager instance;
    [Tooltip("Drag the AppVoiceExperience component from your scene here.")]
    public AppVoiceExperience voiceExperience;
    public CupDispenser cupDispenser;

    private bool isListening = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        voiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnTranscriptReceived);
        voiceExperience.VoiceEvents.OnStoppedListening.AddListener(ResetListeningState);
        voiceExperience.VoiceEvents.OnError.AddListener(OnVoiceError);
    }

    private void OnDisable()
    {
        voiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnTranscriptReceived);
        voiceExperience.VoiceEvents.OnStoppedListening.RemoveListener(ResetListeningState);
        voiceExperience.VoiceEvents.OnError.RemoveListener(OnVoiceError);
    }

    public void StartListening()
    {
        if (!isListening)
        {
            Debug.Log("Walkie-Talkie ON: Listening for voice commands...");
            voiceExperience.Activate();
            isListening = true;
        }
    }

    public void StopListening()
    {
        if (isListening)
        {
            Debug.Log("Walkie-Talkie OFF: Stopped listening.");
            voiceExperience.Deactivate();
        }
    }

    private void OnTranscriptReceived(string transcript)
    {
        string lowerText = transcript.ToLower();
        Debug.Log($"Player said: {lowerText}");

        if (lowerText.Contains("refill"))
        {
            if (cupDispenser != null)
            {
                cupDispenser.RefillDispenser();
            }
        }

        if (lowerText.Contains("please") && lowerText.Contains("god"))
        {
            if (EventManager.instance != null && EventManager.instance.currentEvent == Events.HotWeather)
            {
                Debug.Log("The heavens have answered! Hot weather ended.");
                EventManager.instance.ResolveCurrentEvent();
            }
            else
            {
                Debug.Log("You begged, but the weather wasn't hot anyway.");
            }
        }
    }

    private void ResetListeningState()
    {
        isListening = false;
    }

    private void OnVoiceError(string error, string message)
    {
        Debug.LogError($"Voice SDK Error: {error} - {message}");
        isListening = false;
    }
}
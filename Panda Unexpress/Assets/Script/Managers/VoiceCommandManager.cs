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
    }

    private void OnDisable()
    {
        voiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnTranscriptReceived);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            StartListening();
        }
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

    private void OnTranscriptReceived(string transcript)
    {
        isListening = false;

        string lowerText = transcript.ToLower();
        Debug.Log($"Player said: {lowerText}");

        if (lowerText.Contains("refill") || lowerText.Contains("cups"))
        {
            if (cupDispenser != null)
            {
                cupDispenser.RefillDispenser();
            }
        }
    }
}
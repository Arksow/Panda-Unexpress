using UnityEngine;
using UnityEngine.InputSystem;
public class voiceTask : MonoBehaviour
{
    public CupDispenser dispenser;
    public TutorialManager tutorialManager;
    public int stepIndex;
    private bool wasPressedLastFrame = false;
    [Header("DEBUG")]
    public bool debugSkipWithButton = true;
    // Better to use an Action Reference from your Input Actions Asset
    public InputActionReference debugSkipReference;

    private bool completed = false;

    void OnEnable()
    {
        dispenser.OnDispenserRefilled += CompleteTask;

        
        if (debugSkipReference != null)
            debugSkipReference.action.Enable();

        wasPressedLastFrame = false;
    }

    void OnDisable()
    {
        dispenser.OnDispenserRefilled -= CompleteTask;
    }

    void Update()
    {
        if (completed || !debugSkipWithButton || debugSkipReference == null) return;

        //debug check NOT added in build
        // Check the ACTUAL current state of the button
        bool isCurrentlyPressed = debugSkipReference.action.IsPressed();

        // Only trigger if it is pressed NOW but was NOT pressed last frame
        if (isCurrentlyPressed && !wasPressedLastFrame)
        {
            Debug.Log($"DEBUG: Skip triggered for step {stepIndex}");
            CompleteTask();
        }

        // Save the state for the next frame
        wasPressedLastFrame = isCurrentlyPressed;
    }

    private void CompleteTask()
    {
        if (completed) return;
        completed = true;
        tutorialManager.CompleteStep(stepIndex);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
public class voiceTask : MonoBehaviour
{
    public CupDispenser dispenser;
    public TutorialManager tutorialManager;
    public int stepIndex;

    [Header("DEBUG")]
    public bool debugSkipWithButton = true;

    private InputAction skipAction;
    private bool completed = false;

    void Awake()
    {
        skipAction = new InputAction(type: InputActionType.Button);

        //bind to controller for debug
        skipAction.AddBinding("<XRController>{RightHand}/primaryButton");
        skipAction.AddBinding("<XRController>{LeftHand}/primaryButton");

        skipAction.performed += OnSkipPressed;
    }

    void OnEnable()
    {
        dispenser.OnDispenserRefilled += CompleteTask;

        if (debugSkipWithButton)
            skipAction.Enable();
    }

    void OnDisable()
    {
        dispenser.OnDispenserRefilled -= CompleteTask;
        skipAction.Disable();
    }

    void Update()
    {
        if (!debugSkipWithButton || completed) return;
    }

    private void OnSkipPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("DEBUG: Skipping voice task (controller)");
        CompleteTask();
    }


    private void CompleteTask()
    {
        if (completed) return;

        completed = true;
        tutorialManager.CompleteStep(stepIndex);
    }
}

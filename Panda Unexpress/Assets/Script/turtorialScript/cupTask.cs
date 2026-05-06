using UnityEngine;
using Oculus.Interaction;

public class cupTask : MonoBehaviour
{
    public TutorialManager manager;
    public int currentTask;

    private Grabbable grab;

    private bool wasGrabbed = false;

    void Awake()
    {
        grab = GetComponent<Grabbable>();

        if (grab == null)
        {
            Debug.LogWarning("Panda Unexpress: cupTask cannot find a Grabbable component on this object!");
        }
    }

    void Update()
    {
        if (grab != null && grab.SelectingPointsCount > 0)
        {
            if (!wasGrabbed)
            {
                wasGrabbed = true;
                OnGrabbed();
            }
        }
        else
        {
            wasGrabbed = false;
        }
    }

    void OnGrabbed()
    {
        if (manager.currentSteps != currentTask)
            return;

        manager.CompleteStep(currentTask);
        Debug.Log("Cup grabbed - tutorial step completed!");
    }
}
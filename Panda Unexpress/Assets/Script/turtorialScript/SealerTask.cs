using UnityEngine;

public class SealerTask : MonoBehaviour
{
    public int stepID; // set this to your seal step number in inspector
    private TutorialManager tutorial;

    private void Start()
    {
        tutorial = FindAnyObjectByType<TutorialManager>();
    }

    private void OnEnable()
    {
        CupSealer.OnCupSealed += HandleSeal;
    }

    private void OnDisable()
    {
        CupSealer.OnCupSealed -= HandleSeal;
    }

    private void HandleSeal()
    {
        if (tutorial.GetCurrentStep() == stepID)
        {
            tutorial.CompleteStep(stepID);
        }
    }
}

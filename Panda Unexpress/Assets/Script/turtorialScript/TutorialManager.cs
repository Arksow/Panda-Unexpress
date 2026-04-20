using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public int currentSteps;
    public TMP_Text textUI;

    public string[] steps;

    void Start()
    {
        UpdateSteps();
    }

    public void UpdateSteps()
    {
        textUI.text = steps[currentSteps];

    }
    public void CompleteStep()
    {
        currentSteps++; //go to next task
        if (currentSteps < steps.Length)
        {
            UpdateSteps();
        }
        else
        {
            CompleteTurtorial();
        }
    }

    public void CompleteTurtorial()
    {
        textUI.text = "Completed Turtorial";

        //return back to main scene ya
    }
}

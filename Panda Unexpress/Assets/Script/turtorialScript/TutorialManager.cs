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
        if (currentSteps >= 0 && currentSteps < steps.Length)
            textUI.text = steps[currentSteps];

    }
    public void CompleteStep()
    {

        if (currentSteps >= steps.Length)
            return;



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


    //getter 
    public int GetCurrentStep()
    {
        return currentSteps;
    }

    //in case restart
    public void ResetTutorial()
    {
        currentSteps = 0;
        UpdateSteps();
    }
}

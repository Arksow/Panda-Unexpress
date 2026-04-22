using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class TutorialStepData //open list here yes
{
    [TextArea]
    public string stepText;
    public TaskHighlight[] highlights;
}



public class TutorialManager : MonoBehaviour
{
    public int currentSteps;
    public TMP_Text textUI;

    public TutorialStepData[] steps;

    void Start()
    {
        UpdateSteps();


        Invoke(nameof(AutoAdvanceFromWelcome), 6f);//complete step 0 auto to move on, cool thing i learnt
    }
    void AutoAdvanceFromWelcome() //y
    {
        CompleteStep(0);
    }
    public void UpdateSteps()
    {
        if (currentSteps < 0 || currentSteps >= steps.Length)
            return;

        
        textUI.text = steps[currentSteps].stepText;

       
        DisableAllHighlights();


        //turn on
        foreach (var h in steps[currentSteps].highlights)
        {
            if (h != null)
                h.SetHighlight(true);
        }

    }

    private void DisableAllHighlights()
    {
        foreach (var step in steps)
        {
            foreach (var h in step.highlights)
            {
                if (h != null)
                    h.SetHighlight(false);
            }
        }
    }

    public void CompleteStep(int stepID)
    {
        
        if (stepID != currentSteps)
            return;

        currentSteps++;

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
        DisableAllHighlights();
        //return back to main scene ya,or maybe direct to game dk yet
    }


    //getter 
    public int GetCurrentStep()
    {
        return currentSteps; //put here from old project in cse
    }

    //in case restart
    public void ResetTutorial()
    {
        currentSteps = 0;
        UpdateSteps();
    }
}

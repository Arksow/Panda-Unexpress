using UnityEngine;

public class bobaTask : MonoBehaviour
{
    public TutorialManager manager;
    public CupData cup;
    public int bobaStep;

    void OnEnable()
    {
        if (cup != null)
            cup.OnBobaAdded += OnBobaAdded;
    }

    void OnDisable()
    {
        if (cup != null)
            cup.OnBobaAdded -= OnBobaAdded;
    }

    void OnBobaAdded()
    {
        manager.CompleteStep(bobaStep);
        Debug.Log("Boba collected - tutorial step completed");
    }
}

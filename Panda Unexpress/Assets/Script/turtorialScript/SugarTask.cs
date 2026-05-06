using UnityEngine;

public class SugarTask : MonoBehaviour
{
    public TutorialManager manager;
    public int sugarStep;

    public CupData cup;
    //public int sugarStep;

    void OnEnable()
    {
        if (cup != null)
            cup.OnSugarAdded += OnSugarAdded;
    }

    void OnDisable()  
    {
        if (cup != null)
            cup.OnSugarAdded -= OnSugarAdded;
    }

    void OnSugarAdded()
    {
        manager.CompleteStep(sugarStep);
        Debug.Log("Sugar added - tutorial step completed");
    }
}

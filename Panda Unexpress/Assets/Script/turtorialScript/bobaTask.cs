using UnityEngine;

public class bobaTask : BobaBin
{
    public TutorialManager manager;
    public int bobaStep;

    protected override void OnParticleTrigger()
    {
        base.OnParticleTrigger();

        // if particles entered, scoop received boba
        if (targetScoop != null && targetScoop.heldParticles > 0)
        {
            manager.CompleteStep(bobaStep);
            Debug.Log("Boba collected - tutorial step completed");
        }
    }
}

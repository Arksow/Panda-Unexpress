using UnityEngine;

public class SprigotTask : SpigotDispenser
{
    public TutorialManager manager;
    public int pourLiquidStep;
    private float pourTimer = 0f;
    public float requiredPourTime = 1.2f; // seconds of real pouring needed

    protected override void Update()
    {
        base.Update();

        if (!cupSocket.HasItem())
        {
            pourTimer = 0f;
            return;
        }

        CupData cup = cupSocket.GetSocketItem();
        if (cup == null) return;

        
        if (isPouring)
        {
            pourTimer += Time.deltaTime;

            if (pourTimer >= requiredPourTime)
            {
                manager.CompleteStep(pourLiquidStep);
                enabled = false; 
            }
        }
        else
        {
            pourTimer = 0f;
        }
    }
}

using UnityEngine;

public class SprigotTask : SpigotDispenser
{
    public TutorialManager manager;
    public int pourLiquidStep;

    protected override void Update()
    {
        base.Update();


        if (cupSocket.HasItem())
        {
            CupData cup = cupSocket.GetSocketItem();

            if (cup != null)
            {

                manager.CompleteStep(pourLiquidStep);
            }
        }
    }
}

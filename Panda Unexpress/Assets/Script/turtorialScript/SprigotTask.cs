using UnityEngine;

public class SprigotTask : SpigotDispenser
{
    public TutorialManager manager;
    public int pourLiquidStep;

    protected override void Update()
    {
        base.Update();


        if (cupSocket.HasCup())
        {
            CupData cup = cupSocket.currentCup;

            if (cup != null)
            {

                manager.CompleteStep(pourLiquidStep);
            }
        }
    }
}

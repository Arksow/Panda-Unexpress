using UnityEngine;

public class SprigotTask : SpigotDispenser
{
    public TutorialManager manager;
    public int pourLiquidStep;

    protected override void Update()
    {
        base.Update(); 

      
        if (cupSocket.hasSelection && isTriggerPulled)
        {
            var cupInteractable = cupSocket.interactablesSelected[0];
            CupData cup = cupInteractable.transform.GetComponent<CupData>();

            if (cup != null && !cup.isTrashCup)
            {
             
                manager.CompleteStep(pourLiquidStep);
            }
        }
    }
}

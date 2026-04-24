using UnityEngine;

public class IceScoopTask : IceScoop
{
    public TutorialManager manager;
    public int getIceStep;   
    public int pourIceStep;
    protected override void OnTriggerEnter(Collider other) //inherit
    {
        base.OnTriggerEnter(other);

        if (other.CompareTag("IceBin"))
        {
            //taskDone = true;
            manager.CompleteStep(getIceStep);
            Debug.Log("Ice collected - tutorial step completed");
        }
    }

    protected override void TryPourIce()
    {
   
        base.TryPourIce();

       
        manager.CompleteStep(pourIceStep);
        Debug.Log("Ice poured into cup - tutorial step completed");
    }
}

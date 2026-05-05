using UnityEngine;

[RequireComponent(typeof(OVRGrabbable))]
public class SpigotDispenser : MonoBehaviour
{
    [Header("Dispenser Settings")]
    public LiquidBase barrelLiquidType;
    public CupSocket cupSocket;
    public ParticleSystem liquidStream;
    public float fillSpeed = 0.2f;

    private OVRGrabbable grabbable;
    public bool isTriggerPulled = false;

    void Awake()
    {
        grabbable = GetComponent<OVRGrabbable>();
        if (liquidStream != null) liquidStream.Stop();
    }

    protected virtual void Update()
    {
        if (grabbable.isGrabbed)
        {
            float leftTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            float rightTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

            isTriggerPulled = (leftTrigger > 0.5f) || (rightTrigger > 0.5f);
        }
        else
        {
            isTriggerPulled = false;
        }

        if (isTriggerPulled && !liquidStream.isPlaying) liquidStream.Play();
        else if (!isTriggerPulled && liquidStream.isPlaying) liquidStream.Stop();

        if (isTriggerPulled && cupSocket.HasCup())
        {
            CupData cup = cupSocket.currentCup;

            if (cup != null && !cup.isTrashCup)
            {
                cup.AddLiquid(barrelLiquidType, fillSpeed * Time.deltaTime);
            }
        }

    }
}
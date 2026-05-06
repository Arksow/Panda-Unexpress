using UnityEngine;
using Oculus.Interaction;

public class SpigotDispenser : MonoBehaviour
{
    [Header("Dispenser Settings")]
    public LiquidBase barrelLiquidType;

    public MetaSocket cupSocket;

    public ParticleSystem liquidStream;
    public float fillSpeed = 0.2f;

    private Grabbable grabbable;
    public bool isTriggerPulled = false;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        if (liquidStream != null) liquidStream.Stop();
    }

    protected virtual void Update()
    {
        if (grabbable != null && grabbable.SelectingPointsCount > 0)
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

        if (isTriggerPulled && cupSocket.HasItem())
        {
            CupData cup = cupSocket.GetSocketItem();

            if (cup != null)
            {
                float fillAmount = fillSpeed * Time.deltaTime;
                cup.AddLiquid(barrelLiquidType, fillAmount);
            }
        }
    }
}
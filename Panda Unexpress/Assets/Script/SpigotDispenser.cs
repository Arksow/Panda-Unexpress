using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SpigotDispenser : MonoBehaviour
{
    [Header("Dispenser Settings")]
    public LiquidBase barrelLiquidType;
    public XRSocketInteractor cupSocket;
    public ParticleSystem liquidStream;
    public float fillSpeed = 0.2f;

    private XRGrabInteractable grabInteractable;
    public bool isTriggerPulled = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        liquidStream.Stop();

        grabInteractable.activated.AddListener(OnTriggerPressed);
        grabInteractable.deactivated.AddListener(OnTriggerReleased);
    }

    void OnDestroy()
    {
        grabInteractable.activated.RemoveListener(OnTriggerPressed);
        grabInteractable.deactivated.RemoveListener(OnTriggerReleased);
    }

    private void OnTriggerPressed(ActivateEventArgs args)
    {
        isTriggerPulled = true;
        liquidStream.Play();
    }

    private void OnTriggerReleased(DeactivateEventArgs args)
    {
        isTriggerPulled = false;
        liquidStream.Stop();
    }

    protected virtual void Update()
    {
        if (isTriggerPulled && cupSocket.hasSelection)
        {
            IXRSelectInteractable cupInteractable = cupSocket.interactablesSelected[0];
            CupData cup = cupInteractable.transform.GetComponent<CupData>();

            if (cup != null && !cup.isTrashCup)
            {
                cup.AddLiquid(barrelLiquidType, fillSpeed * Time.deltaTime);
            }
        }
    }
}
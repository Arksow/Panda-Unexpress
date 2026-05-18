using Oculus.Interaction;
using UnityEngine;

public class MetaSocket : MonoBehaviour
{
    public Transform attachPoint;
    public string targetLayer = "Cup";

    public GameObject hologram;

    private Grabbable currentItem;
    private Rigidbody currentRigidbody;

    private Grabbable hoveringItem;

    private void Start()
    {
        if (hologram != null)
        {
            hologram.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentItem != null) return;

        if (LayerMask.LayerToName(other.gameObject.layer) != targetLayer) return;

        Grabbable enteringGrabbable = other.GetComponentInParent<Grabbable>();
        if (enteringGrabbable != null)
        {
            hoveringItem = enteringGrabbable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hoveringItem != null && other.GetComponentInParent<Grabbable>() == hoveringItem)
        {
            hoveringItem = null;

            if (hologram != null)
            {
                hologram.SetActive(false);
            }
        }
    }

    private void Update()
    {
        // Notice we are NO LONGER locking the position here.
        // The Rigidbody is kinematic, so it floats in place natively. 
        // This stops your script from fighting the Meta SDK's Two-Handed movement math!

        if (hoveringItem != null && currentItem == null)
        {
            if (hoveringItem.SelectingPointsCount > 0)
            {
                if (hologram != null)
                {
                    hologram.SetActive(true);
                    hologram.transform.position = attachPoint.position;
                    hologram.transform.rotation = attachPoint.rotation;
                }
            }
            else
            {
                if (hologram != null) hologram.SetActive(false);

                SocketIt(hoveringItem, hoveringItem.GetComponent<Rigidbody>());
                hoveringItem = null;
            }
        }
    }

    private void SocketIt(Grabbable newObj, Rigidbody objRb)
    {
        currentItem = newObj;
        currentRigidbody = objRb;

        if (currentRigidbody != null)
        {
            currentRigidbody.linearVelocity = Vector3.zero;
            currentRigidbody.angularVelocity = Vector3.zero;
            currentRigidbody.isKinematic = true;
        }

        // Snap to position
        currentItem.transform.position = attachPoint.position;
        currentItem.transform.rotation = attachPoint.rotation;

        // NEW: Parent the item to the socket so it moves with it
        // The 'true' parameter ensures the cup doesn't magically shrink or grow 
        // if your moving socket has a weird scale applied to it.
        currentItem.transform.SetParent(attachPoint, true);

        currentItem.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void ReleaseIt()
    {
        if (currentItem != null)
        {
            currentItem.WhenPointerEventRaised -= HandlePointerEvent;

            // NEW: Un-parent the item so it is back in the main world space
            currentItem.transform.SetParent(null, true);
        }

        if (currentRigidbody != null)
        {
            currentRigidbody.isKinematic = false;
            currentRigidbody.WakeUp();
            currentRigidbody.linearVelocity = Vector3.zero;
            currentRigidbody.angularVelocity = Vector3.zero;
        }

        hoveringItem = currentItem;
        currentItem = null;
        currentRigidbody = null;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        // Intercept the exact frame the user pulls the grab trigger
        if (evt.Type == PointerEventType.Select && currentItem != null)
        {
            ReleaseIt();
        }
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    public CupData GetSocketItem()
    {
        if (currentItem != null)
        {
            return currentItem.GetComponent<CupData>();
        }
        return null;
    }

    public void ForceSocket(Grabbable newObj)
    {
        hoveringItem = null;
        if (hologram != null) hologram.SetActive(false);

        if (currentItem != null)
        {
            currentItem.WhenPointerEventRaised -= HandlePointerEvent;
        }

        SocketIt(newObj, newObj.GetComponent<Rigidbody>());
    }

    public bool IsBlocked()
    {
        return currentItem != null || hoveringItem != null;
    }
}
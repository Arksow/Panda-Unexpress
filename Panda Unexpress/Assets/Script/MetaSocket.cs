using Oculus.Interaction;
using UnityEngine;
using System.Collections;

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
        // 1. Manually lock position here to support Moving Sockets WITHOUT using SetParent().
        // This prevents the Meta SDK from breaking if the Dispenser has a customized scale!
        if (currentItem != null)
        {
            currentItem.transform.position = attachPoint.position;
            currentItem.transform.rotation = attachPoint.rotation;
        }

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

        currentItem.transform.position = attachPoint.position;
        currentItem.transform.rotation = attachPoint.rotation;

        currentItem.WhenPointerEventRaised += HandlePointerEvent;
    }

    private void HandlePointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select && currentItem != null)
        {
            ReleaseIt();
        }
    }

    private void ReleaseIt()
    {
        // Store references before nulling them out
        var releasingItem = currentItem;
        var releasingRb = currentRigidbody;

        releasingItem.WhenPointerEventRaised -= HandlePointerEvent;
        hoveringItem = releasingItem;

        // Instantly null currentItem so the Update() loop STOPS forcing the position.
        // This allows your hand to pull it away smoothly.
        currentItem = null;
        currentRigidbody = null;

        // 2. Defer turning on physics until the Meta SDK is done doing its Two-Handed Math
        if (releasingRb != null && gameObject.activeInHierarchy)
        {
            StartCoroutine(RestorePhysicsRoutine(releasingRb));
        }
    }

    private IEnumerator RestorePhysicsRoutine(Rigidbody rb)
    {
        // Wait until the very end of the frame. The Meta SDK grab math is now safely finished.
        yield return new WaitForEndOfFrame();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.WakeUp(); // Wake up the colliders so triggers/spherecasts work instantly
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
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
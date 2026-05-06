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

        if (currentItem != null && currentItem.SelectingPointsCount > 0)
        {
            ReleaseIt();
        }
    }

    private void SocketIt(Grabbable newObj, Rigidbody objRb)
    {
        currentItem = newObj;
        currentRigidbody = objRb;
        
        if (currentRigidbody != null)
        {
            currentRigidbody.isKinematic = true;
            currentRigidbody.linearVelocity = Vector3.zero;
            currentRigidbody.angularVelocity = Vector3.zero;
        }

        currentItem.transform.position = attachPoint.position;
        currentItem.transform.rotation = attachPoint.rotation;
    }

    private void ReleaseIt()
    {
        if (currentRigidbody != null)
        {
            currentRigidbody.isKinematic = false;
        }
        hoveringItem = currentItem;
        currentItem = null;
        currentRigidbody = null;
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
}

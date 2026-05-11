using UnityEngine;

public class IceScoop : MonoBehaviour
{
    public GameObject Ice;

    public Transform pourPoint;
    public LayerMask cupLayer;
    public float tiltThreshold = 0.5f;
    public float pourRadius = 0.25f;

    private bool isFull = false;

    void Start()
    {
        Ice.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceBin") && !isFull)
        {
            isFull = true;
            Ice.SetActive(true);
        }
    }

    void Update()
    {
        if (isFull)
        {
            float tilt = Vector3.Dot(-transform.right, Vector3.down);
            if (tilt > tiltThreshold)
            {
                TryPourIce();
            }
        }
    }

    protected virtual void TryPourIce()
    {
        Collider[] hitColliders = Physics.OverlapSphere(pourPoint.position, pourRadius, cupLayer);
        foreach (var hitCollider in hitColliders)
        {
            CupData cup = hitCollider.GetComponent<CupData>();
            if (cup != null)
            {
                cup.AddIceScoop();

                isFull = false;
                Ice.SetActive(false);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pourPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(pourPoint.position, pourRadius);
        }
    }
}
using UnityEngine;

public class IceScoop : MonoBehaviour
{
    public GameObject emptyScoopMesh;
    public GameObject fullScoopMesh;

    public Transform pourPoint;
    public LayerMask cupLayer;
    public float tiltThreshold = 0.5f;

    private bool isFull = false;

    void Start()
    {
        fullScoopMesh.SetActive(false);
        emptyScoopMesh.SetActive(true);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceBin") && !isFull)
        {
            isFull = true;
            fullScoopMesh.SetActive(true);
            emptyScoopMesh.SetActive(false);
        }
    }

    void Update()
    {
        if (isFull)
        {
            float tilt = Vector3.Dot(transform.up, Vector3.down);
            if (tilt > tiltThreshold)
            {
                TryPourIce();
            }
        }
    }

    protected virtual void TryPourIce()
    {
        Collider[] hitColliders = Physics.OverlapSphere(pourPoint.position, 0.15f, cupLayer);
        foreach (var hitCollider in hitColliders)
        {
            CupData cup = hitCollider.GetComponent<CupData>();
            if (cup != null)
            {
                cup.AddIceScoop();

                isFull = false;
                fullScoopMesh.SetActive(false);
                emptyScoopMesh.SetActive(true);
                return;
            }
        }
    }
}
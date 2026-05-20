using UnityEngine;

public class IceScoop : MonoBehaviour
{
    public GameObject Ice;

    public Transform pourPoint;
    public LayerMask cupLayer;
    public float tiltThreshold = 0.5f;
    public float pourRadius = 0.25f;

    private bool isFull = false;
    public AudioClip iceSound;

    void Start()
    {
        Ice.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IceBin") && !isFull)
        {
            isFull = true;
            AudioController.Instance?.PlayGlobalSFX(iceSound, 1.5f);
            Ice.SetActive(true);
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
        bool pouredInCup = false;
        Collider[] hitColliders = Physics.OverlapSphere(pourPoint.position, pourRadius, cupLayer);

        foreach (var hitCollider in hitColliders)
        {
            CupData cup = hitCollider.GetComponentInParent<CupData>();
            if (cup != null)
            {
                cup.AddIceScoop();
                pouredInCup = true;
                break;
            }
        }

        if (!pouredInCup)
        {
            Debug.Log("Ice dropped on the floor!");
        }

        isFull = false;
        AudioController.Instance?.PlayGlobalSFX(iceSound, 1.5f);
        Ice.SetActive(false);
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
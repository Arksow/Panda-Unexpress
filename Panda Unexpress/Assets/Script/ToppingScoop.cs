using UnityEngine;

public class ToppingScoop : MonoBehaviour
{
    public Transform bowlCenter;
    public float dropAngleThreshold = 0.2f;

    private GameObject currentPearl;
    private Rigidbody pearlRb;

    void OnTriggerStay(Collider other)
    {
        if (currentPearl == null)
        {
            BobaPearl touchedPearl = other.GetComponent<BobaPearl>();

            if (touchedPearl != null && !touchedPearl.isScooped)
            {
                touchedPearl.isScooped = true;

                currentPearl = touchedPearl.gameObject;
                currentPearl.transform.SetParent(bowlCenter);
                currentPearl.transform.localPosition = Vector3.zero;
                pearlRb = currentPearl.GetComponent<Rigidbody>();
                if (pearlRb != null)
                {
                    pearlRb.isKinematic = true;
                }

                Debug.Log("Physically caught a Boba Ball!");
            }
        }
    }

    void Update()
    {
        if (currentPearl != null && pearlRb != null && pearlRb.isKinematic)
        {
            if (Vector3.Dot(transform.up, Vector3.up) < dropAngleThreshold)
            {
                currentPearl.transform.SetParent(null);
                pearlRb.isKinematic = false;

                pearlRb = null;
                currentPearl = null;
            }
        }
    }
}
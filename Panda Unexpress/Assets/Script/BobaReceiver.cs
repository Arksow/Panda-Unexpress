using UnityEngine;

public class BobaReceiver : MonoBehaviour
{
    private CupData cupData;

    void Start()
    {
        cupData = GetComponentInParent<CupData>();
    }

    void OnTriggerEnter(Collider other)
    {
        BobaPearl incomingPearl = other.GetComponent<BobaPearl>();

        if (incomingPearl != null && incomingPearl.isScooped)
        {
            Destroy(incomingPearl.gameObject);

            if (cupData != null)
            {
                cupData.AddBobaParticles(30);
                Debug.Log("1 Boba Scoop added to cup!");
            }
        }
    }
}
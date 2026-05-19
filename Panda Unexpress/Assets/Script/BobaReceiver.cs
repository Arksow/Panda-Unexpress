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
        ToppingItem incomingTopping = other.GetComponent<ToppingItem>();

        if (incomingTopping != null && !cupData.isSealed)
        {
            // Destroy the falling physical object
            Destroy(incomingTopping.gameObject);

            if (cupData != null)
            {
                // Add the topping directly to the cup's data
                cupData.AddTopping(incomingTopping.toppingType, 1);
                Debug.Log($"1 Scoop of {incomingTopping.toppingType} added to cup!");
            }
        }
    }
}
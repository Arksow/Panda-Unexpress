using UnityEngine;

public class Counter : MonoBehaviour
{
    [Header("Customer Reference")]
    public CustomerAI customerManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cup"))
        {
            CupData cup = other.GetComponent<CupData>();

            if (cup != null && customerManager != null)
            {
                customerManager.receivedOrder = cup;
                customerManager.CheckOrder(cup);
                Destroy(other.gameObject);
            }
        }
    }
}
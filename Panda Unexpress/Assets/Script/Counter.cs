using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private List<CustomerAI> customersAtCounter = new List<CustomerAI>();

    private void OnTriggerEnter(Collider other)
    {
        CustomerAI customer = other.GetComponent<CustomerAI>();

        if (customer != null)
        {
            if (!customersAtCounter.Contains(customer))
                customersAtCounter.Add(customer);

            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Cup"))
        {
            CupData cup = other.GetComponent<CupData>();

            if (cup != null && customersAtCounter.Count > 0)
            {
                CustomerAI targetCustomer = customersAtCounter[0];
                targetCustomer.CheckOrder(cup);

                Destroy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CustomerAI customer = other.GetComponent<CustomerAI>();

        if (customer != null)
        {
            customersAtCounter.Remove(customer);
        }
    }
}
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderUIManager : MonoBehaviour
{
    public TextMeshProUGUI orderText;

    private List<CustomerAI> customers = new List<CustomerAI>();

    public void AddCustomer(CustomerAI customer)
    {
        if (!customers.Contains(customer))
            customers.Add(customer);

        UpdateUI();
    }

    public void RemoveCustomer(CustomerAI customer)
    {
        customers.Remove(customer);

        if (customers.Count == 0)
        {
            orderText.text = "Order List";
        }
        else
        {
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        string text = "";
        text += "Order List:\n";

        foreach (var c in customers)
        {
            if (c == null) continue;

            for (int i = 0; i < c.currentOrders.Count; i++)
            {
                var order = c.currentOrders[i];

                text += $" Drink {i + 1}:\n";
                text += "- Base: " + order.base1 + "\n";
                text += "- Base: " + order.base2 + "\n";
                text += "- Sugar: " + order.sugarPercent + "%\n";
                text += "- " + GetIceText(order.iceAmount) + "\n";
                text += "- " + GetBobaText(order.bobaAmount) + "\n\n";
            }

            text += "-----------------\n";
        }

        orderText.text = text;
    }

    string GetIceText(int ice)
    {
        switch (ice)
        {
            case 0: return "No Ice";
            case 1: return "Less Ice";
            case 2: return "Regular Ice";
            case 3: return "More Ice";
            default: return "Unknown";
        }
    }

    string GetBobaText(int boba)
    {
        switch (boba)
        {
            case 0: return "No Boba";
            case 1: return "Less Boba";
            case 2: return "Normal Boba";
            case 3: return "More Boba";
            default: return "Unknown";
        }
    }
}
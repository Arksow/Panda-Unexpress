using TMPro;
using UnityEngine;

public class OrderUIManager : MonoBehaviour
{
    public TextMeshProUGUI orderText;

    private CustomerAI currentCustomer;

    public void SetCustomer(CustomerAI customer)
    {
        currentCustomer = customer;
        UpdateUI();
    }

    public void ClearCustomer(CustomerAI customer)
    {
        if (currentCustomer == customer)
        {
            currentCustomer = null;
            orderText.text = "";
        }
    }

    public void UpdateUI()
    {
        if (currentCustomer == null)
        {
            orderText.text = "";
            return;
        }

        string text = "";

        for (int i = 0; i < currentCustomer.currentOrders.Count; i++)
        {
            var order = currentCustomer.currentOrders[i];

            text += "" + order.base1 + " " + order.base2 + "\n";
            text += "- Sugar Level: " + order.sugarPercent + "%\n";
            text += "- " + GetIceText(order.iceAmount) + "\n";
            text += "- " + GetBobaText(order.bobaAmount) + "\n\n";
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
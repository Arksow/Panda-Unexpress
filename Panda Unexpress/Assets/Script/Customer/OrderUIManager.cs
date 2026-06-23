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

            text += "" + order.base1 + " & " + order.base2 + "\n";

            if (order.sugarType == SugarType.None)
            {
                text += "- No Sweetener\n";
            }
            else
            {
                text += "- " + order.sugarType.ToString() + ": " + order.sugarPercent + "%\n";
            }

            text += "- " + GetScoopText(order.iceAmount, "Ice") + "\n";
            if (order.toppingType == ToppingType.None)
            {
                text += "- No Topping\n\n";
            }
            else
            {
                int toppingAmount = order.toppingType == ToppingType.Boba
                    ? order.bobaAmount
                    : order.aloeAmount;
                text += "- " + GetScoopText(toppingAmount, order.toppingType.ToString()) + "\n\n";
            }
        }

        orderText.text = text;
    }

    string GetScoopText(int amount, string ingredientName)
    {
        if (amount == 0)
        {
            return "No " + ingredientName;
        }
        else if (amount == 1)
        {
            return "1 Scoop of " + ingredientName;
        }
        else
        {
            return amount + " Scoops of " + ingredientName;
        }
    }
}

using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public CustomerOrder GenerateOrder()
    {
        CustomerOrder order = new CustomerOrder();

        //Base
        SetBaseCombination(order);

        //Sugar
        bool hasSugarUpgrade = PlayerPrefs.GetInt("SugarUpgrade", 0) == 1;
        order.sugarType = (SugarType)Random.Range(0, hasSugarUpgrade ? 4 : 3);
        order.sugarPercent = GetRandomSugar(order.sugarType);

        //Ice
        order.iceAmount = GetRandomIce();

        //Boba
        bool hasBobaUpgrade = PlayerPrefs.GetInt("BobaUpgrade", 0) == 1;
        order.toppingType = (ToppingType)Random.Range(0,hasBobaUpgrade ? 3 : 2);
        order.bobaAmount = GetRandomBoba(order.toppingType);

        return order;
    }

    //Base Combinations
    void SetBaseCombination(CustomerOrder order)
    {
        bool hasChocolateUpgrade = false;

        if (PlayerPrefs.GetInt("BaseUpgrade", 0) == 1)
        {
            hasChocolateUpgrade = true;
        }

        int comboCount = 3;

        if (hasChocolateUpgrade)
        {
            comboCount = 6;
        }

        int combo = Random.Range(0, comboCount);

        switch (combo)
        {
            case 0:
                order.base1 = LiquidBase.Matcha;
                order.base2 = LiquidBase.Milk;
                break;

            case 1:
                order.base1 = LiquidBase.Matcha;
                order.base2 = LiquidBase.Tea;
                break;

            case 2:
                order.base1 = LiquidBase.Milk;
                order.base2 = LiquidBase.Tea;
                break;
            case 3:
                order.base1 = LiquidBase.Chocolate;
                order.base2 = LiquidBase.Matcha;
                break;

            case 4:
                order.base1 = LiquidBase.Chocolate;
                order.base2 = LiquidBase.Milk;
                break;

            case 5:
                order.base1 = LiquidBase.Chocolate;
                order.base2 = LiquidBase.Tea;
                break;
        }
    }

    //Sugar values
    float GetRandomSugar(SugarType sugarType)
    {
        if (sugarType == SugarType.None)
        {
            return 0;
        }

        int[] sugarOptions = { 25, 50, 75, 100 };
        return sugarOptions[Random.Range(0, sugarOptions.Length)];
    }

    //Ice values
    int GetRandomIce()
    {
        float roll = Random.value;

        if (roll < 0.30f) return 0;
        if (roll < 0.65f) return 1;
        if (roll < 0.90f) return 2;
        return 3;
    }

    //Boba values
    int GetRandomBoba(ToppingType bobaType)
    {
        float roll = Random.value;

        if (roll < 0.20f) return 0;
        if (roll < 0.50f) return 1;
        if (roll < 0.80f) return 2;
        return 3;
    }
}
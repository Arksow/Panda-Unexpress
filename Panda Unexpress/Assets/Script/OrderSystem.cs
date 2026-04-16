using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    public CustomerOrder GenerateOrder()
    {
        CustomerOrder order = new CustomerOrder();

        //Base
        order.base1 = GetRandomBase();
        order.base2 = GetRandomBaseDifferent(order.base1);

        //Sugar
        order.sugarType = (SugarType)Random.Range(0, 3);
        order.sugarPercent = GetRandomSugar(order.sugarType);

        //Ice
        order.iceAmount = GetRandomIce();

        //Boba
        order.bobaAmount = GetRandomBoba();

        return order;
    }

    LiquidBase GetRandomBase()
    {
        return (LiquidBase)Random.Range(1, 5);
    }

    LiquidBase GetRandomBaseDifferent(LiquidBase first)
    {
        LiquidBase second;

        do
        {
            second = GetRandomBase();
        }
        while (second == first);

        return second;
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
    int GetRandomBoba()
    {
        float roll = Random.value;
        if (roll < 0.30f) return 0;
        if (roll < 0.65f) return 1;
        if (roll < 0.90f) return 2;
        return 3;
    }
}
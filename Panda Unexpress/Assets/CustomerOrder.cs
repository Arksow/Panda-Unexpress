using UnityEngine;

[System.Serializable]
public class CustomerOrder
{
    public LiquidBase base1;
    public LiquidBase base2;

    public SugarType sugarType;
    public float sugarPercent;

    public int iceAmount;

    public ToppingType toppingType;
    public int bobaAmount;
    public int aloeAmount;
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Drink Recipe", menuName = "Panda Unexpress/Drink Recipe")]
public class DrinkRecipe : ScriptableObject
{
    public string drinkName;
    public LiquidBase requiredBase1;
    public LiquidBase requiredBase2;

    public bool MatchesRecipe(LiquidBase cupBase1, LiquidBase cupBase2)
    {
        return (requiredBase1 == cupBase1 && requiredBase2 == cupBase2) ||
               (requiredBase1 == cupBase2 && requiredBase2 == cupBase1);
    }
}
using UnityEngine;

public class TextureManager : MonoBehaviour
{
    public static TextureManager Instance;

    [System.Serializable]
    public class BaseTextureEntry
    {
        public LiquidBase baseType;
        public Material liquid;
    }

    [System.Serializable]
    public class DrinkTextureEntry
    {
        public DrinkRecipe recipe;
        public Material liquid;
    }

    public BaseTextureEntry[] baseTextures;
    public DrinkTextureEntry[] drinkTextures;
    public Material trashTexture; // incase

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

 
    //guess u can call it in your sprigot machines

    public void ApplyBaseTexture(Renderer cupRenderer, LiquidBase baseType)
    {
        foreach (var entry in baseTextures)
        {
            if (entry.baseType == baseType)
            {
                cupRenderer.material = entry.liquid;
                return;
            }
        }
    }

    public void ApplyDrinkTexture(Renderer cupRenderer, DrinkRecipe recipe)
    {
        if (recipe == null) return;


        foreach (var entry in drinkTextures)
        {
            if (entry.recipe == recipe)
            {
                cupRenderer.material = entry.liquid;
                return;
            }
        }
    }

    public void ApplyTrashTexture(Renderer cupRenderer)
    {
        cupRenderer.material = trashTexture;
    }
}
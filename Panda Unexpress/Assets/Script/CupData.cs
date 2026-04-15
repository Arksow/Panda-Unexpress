using UnityEngine;
using TMPro;

public enum SugarType { None, Syrup, Honey }
public enum LiquidBase { None, Tea, Matcha, Milk }

public class CupData : MonoBehaviour
{
    [Header("Cup Contents")]
    public int iceScoopCount = 0;
    public int bobaParticleCount = 0;

    public float sugarPercentage = 0f;
    public SugarType currentSugarType = SugarType.None;

    [Header("UI Reference")]
    //public GameObject uiCanvas;
    //public TextMeshProUGUI contentsText;

    [Header("Liquid Bases")]
    public LiquidBase base1 = LiquidBase.None;
    public float base1Amount = 0f;

    public LiquidBase base2 = LiquidBase.None;
    public float base2Amount = 0f;

    public bool isTrashCup = false;

    void Start()
    {
        //uiCanvas.SetActive(false);
        UpdateUI();
    }

    //public void ShowUI() { uiCanvas.SetActive(true); UpdateUI(); }
    //public void HideUI() { uiCanvas.SetActive(false); }

    public void AddIceScoop() { iceScoopCount++; UpdateUI(); }

    public void SetSugar(float percentage, SugarType type)
    {
        sugarPercentage = percentage;
        currentSugarType = type;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int bobaScoops = Mathf.RoundToInt((float)bobaParticleCount / 30f);

        string sugarDisplay = sugarPercentage > 0
            ? $"{currentSugarType}: {Mathf.RoundToInt(sugarPercentage)}%"
            : "Sugar: 0%";

        //contentsText.text = "<u>Cup Contents</u>\n" +
        //                    $"Ice: {iceScoopCount} Scoops\n" +
        //                    $"Boba: {bobaScoops} Scoops\n" +
        //                    $"{sugarDisplay}";
    }

    public void AddLiquid(LiquidBase incomingBase, float amount)
    {
        if (isTrashCup) return;

        if (base1 == LiquidBase.None || base1 == incomingBase)
        {
            base1 = incomingBase;
            base1Amount += amount;
            if (base1Amount > 0.5f) base1Amount = 0.5f;
        }
        else if (base2 == LiquidBase.None || base2 == incomingBase)
        {
            base2 = incomingBase;
            base2Amount += amount;
            if (base2Amount > 0.5f) base2Amount = 0.5f;

            if (base2Amount >= 0.5f)
            {
                ValidateRecipe();
            }
        }
        else
        {
            RuinCup();
        }

        UpdateUI();
    }

    private void ValidateRecipe()
    {
        // Example: Valid Recipe is Tea + Milk (Milk Tea)
        bool isMilkTea = (base1 == LiquidBase.Tea && base2 == LiquidBase.Milk) ||
                         (base1 == LiquidBase.Milk && base2 == LiquidBase.Tea);

        // Example: Valid Recipe is Matcha + Milk (Matcha Latte)
        bool isMatchaLatte = (base1 == LiquidBase.Matcha && base2 == LiquidBase.Milk) ||
                             (base1 == LiquidBase.Milk && base2 == LiquidBase.Matcha);

        if (!isMilkTea && !isMatchaLatte)
        {
            // The combination doesn't exist
            RuinCup();
        }
    }

    private void RuinCup()
    {
        isTrashCup = true;
    }
}
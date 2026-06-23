using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum SugarType { None, Syrup, Honey, BrownSugar }
public enum LiquidBase { None, Tea, Matcha, Milk, Chocolate }
public enum ToppingType { None, Boba, AloeVera }

public class CupData : MonoBehaviour
{
    [Header("Cup Contents")]
    public int iceScoopCount = 0;
    public int bobaScoopCount = 0;
    public int aloeScoopCount = 0;

    public Renderer liquidRenderer;

    [Tooltip("The physical ice cubes sitting at the bottom of the cup")]
    public GameObject iceVisualDry;
    [Tooltip("The ice cubes floating at the top of the liquid")]
    public GameObject iceVisualFloating;
    [Tooltip("The boba pearls at the bottom")]
    public GameObject bobaVisual;
    [Tooltip("The aloe vera chunks at the bottom")]
    public GameObject aloeVisual;

    public float sugarPercentage = 0f;
    public SugarType currentSugarType = SugarType.None;

    [Header("UI Reference")]
    public GameObject uiCanvas;
    public TextMeshProUGUI contentsText;

    [Header("Liquid Bases")]
    public LiquidBase base1 = LiquidBase.None;
    public float base1Amount = 0f;

    public LiquidBase base2 = LiquidBase.None;
    public float base2Amount = 0f;

    public bool isTrashCup = false;
    public string DrinkName = "Unknown Drink";

    public DrinkRecipe[] validRecipes;

    [Header("Hot Weather Event")]
    public float maxMeltTime = 15f;
    private float meltTimer;

    public System.Action OnSugarAdded;
    public System.Action OnBobaAdded;

    public GameObject meltUIContainer;
    public Image meltBarImage;

    public GameObject seal;
    public bool isSealed = false;

    void Start()
    {
        meltTimer = maxMeltTime;
        uiCanvas.SetActive(true);
        UpdateVisuals();
        UpdateUI();
    }

    void Update()
    {
        if (EventManager.instance != null && EventManager.instance.isHotWeather)
        {
            if (iceScoopCount > 0 && !isTrashCup)
            {
                if (meltUIContainer != null) meltUIContainer.SetActive(true);

                meltTimer -= Time.deltaTime;

                if (meltBarImage != null)
                {
                    meltBarImage.fillAmount = meltTimer / maxMeltTime;
                }

                if (meltTimer <= 0f)
                {
                    Debug.Log("Ice melted in the heat! Cup ruined.");
                    RuinCup();
                    meltTimer = maxMeltTime;
                    if (meltUIContainer != null) meltUIContainer.SetActive(false);
                }
            }
            else
            {
                if (meltUIContainer != null) meltUIContainer.SetActive(false);
            }
        }
        else
        {
            if (meltUIContainer != null) meltUIContainer.SetActive(false);
            meltTimer = maxMeltTime;
        }
    }

    public void ShowUI() { uiCanvas.SetActive(true); UpdateUI(); }
    public void HideUI() { uiCanvas.SetActive(false); }

    public void AddIceScoop()
    {
        if (isTrashCup || isSealed) return;

        iceScoopCount++;

        if (EventManager.instance != null && EventManager.instance.isHotWeather)
        {
            meltTimer = maxMeltTime;
        }

        UpdateUI();
        UpdateVisuals();
    }

    public void SetSugar(float percentage, SugarType type)
    {
        if (isTrashCup) return;

        sugarPercentage = percentage;
        currentSugarType = type;
        UpdateUI();
        OnSugarAdded?.Invoke();
    }

    public void UpdateUI()
    {
        string sugarDisplay = sugarPercentage > 0
            ? $"{currentSugarType}: {Mathf.RoundToInt(sugarPercentage)}%"
            : "Sugar: 0%";

        string drinkStatus = "Empty";

        if (isTrashCup) drinkStatus = "<color=red>Ruined (Trash)</color>";
        else if (!string.IsNullOrEmpty(DrinkName) && DrinkName != "Unknown Drink") drinkStatus = $"<color=green>{DrinkName}</color>";
        else if (base1 != LiquidBase.None && base2 != LiquidBase.None) drinkStatus = $"{base1} & {base2}";
        else if (base1 != LiquidBase.None) drinkStatus = base1.ToString();
        else if (base2 != LiquidBase.None) drinkStatus = base2.ToString();

        contentsText.text = "<u>Cup Contents</u>\n" +
                            $"Base: {drinkStatus}\n" +
                            $"Ice: {iceScoopCount} Scoops\n" +
                            $"Boba: {bobaScoopCount} Scoops\n" +
                            $"Aloe: {aloeScoopCount} Scoops\n" +
                            $"{sugarDisplay}";
    }

    public void AddLiquid(LiquidBase incomingBase, float amount)
    {
        if (isTrashCup || isSealed) return;

        if (base1 == LiquidBase.None || base1 == incomingBase)
        {
            base1 = incomingBase;
            if (base1Amount > 0 && base2 == LiquidBase.None)
            {
                TextureManager.Instance?.ApplyBaseTexture(liquidRenderer, base1);
            }
            base1Amount += amount;
            if (base1Amount > 0.5f) base1Amount = 0.5f;
          
        }
        else if (base2 == LiquidBase.None || base2 == incomingBase)
        {
            base2 = incomingBase;
            base2Amount += amount;
            if (base2Amount > 0.5f) base2Amount = 0.5f;
        }
        else
        {
            RuinCup();
            return;
        }

        if (base1Amount + base2Amount >= 1.0f)
        {
            ValidateRecipe();
        }

        UpdateUI();
        UpdateVisuals();
    }

    private void ValidateRecipe()
    {
        if (validRecipes == null) return;

        bool isValidCombo = false;

        foreach (DrinkRecipe recipe in validRecipes)
        {
            if (recipe.MatchesRecipe(base1, base2))
            {
                isValidCombo = true;
                DrinkName = recipe.drinkName;
                TextureManager.Instance?.ApplyDrinkTexture(liquidRenderer, recipe);
                Debug.Log($"Successfully mixed a valid base for: {recipe.drinkName}");
                break;
            }
        }

        if (!isValidCombo)
        {
            RuinCup();
            Debug.Log("Invalid combo! Cup is ruined.");
        }
    }

    private void RuinCup()
    {
        isTrashCup = true;
        TextureManager.Instance?.ApplyTrashTexture(liquidRenderer);
        UpdateUI();
    }

    public void AddTopping(ToppingType type, int amount)
    {
        if (isTrashCup || isSealed) return;

        if (type == ToppingType.Boba)
            bobaScoopCount += amount;
        else if (type == ToppingType.AloeVera)
            aloeScoopCount += amount;

        UpdateUI();
        UpdateVisuals();
        if (type == ToppingType.Boba)
        {
            OnBobaAdded?.Invoke();
        }
    }

    private void UpdateVisuals()
    {
        bool hasLiquid = base1Amount > 0 || base2Amount > 0;
        bool hasIce = iceScoopCount > 0;

        if (iceVisualDry != null)
        {
            iceVisualDry.SetActive(hasIce && !hasLiquid);
        }

        if (iceVisualFloating != null)
        {
            iceVisualFloating.SetActive(hasIce && hasLiquid);
        }

        if (bobaVisual != null)
        {
            bobaVisual.SetActive(bobaScoopCount > 0);
        }

        if (aloeVisual != null)
        {
            aloeVisual.SetActive(aloeScoopCount > 0);
        }
    }
}

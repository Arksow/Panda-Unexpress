using UnityEngine;
using TMPro;
using System.Net;
using System;

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
    public GameObject uiCanvas;
    public TextMeshProUGUI contentsText;

    [Header("Liquid Bases")]
    public LiquidBase base1 = LiquidBase.None;
    public float base1Amount = 0f;

    public LiquidBase base2 = LiquidBase.None;
    public float base2Amount = 0f;

    public bool isTrashCup = false;

    public DrinkRecipe[] validRecipes;

    private float meltTimer = 5f;

    //don't touch
    public System.Action OnSugarAdded;
    public System.Action  OnBobaAdded;
    void Start()
    {
        uiCanvas.SetActive(true);
        UpdateUI();
    }

    public void ShowUI() { uiCanvas.SetActive(true); UpdateUI(); }
    public void HideUI() { uiCanvas.SetActive(false); }

    public void AddIceScoop() { iceScoopCount++; UpdateUI(); }

    public void SetSugar(float percentage, SugarType type)
    {
        sugarPercentage = percentage;
        currentSugarType = type;
        UpdateUI();
        OnSugarAdded?.Invoke();
    }

    private void UpdateUI()
    {
        int bobaScoops = Mathf.RoundToInt((float)bobaParticleCount / 30f);

        string sugarDisplay = sugarPercentage > 0
            ? $"{currentSugarType}: {Mathf.RoundToInt(sugarPercentage)}%"
            : "Sugar: 0%";

        contentsText.text = "<u>Cup Contents</u>\n" +
                            $"Ice: {iceScoopCount} Scoops\n" +
                            $"Boba: {bobaScoops} Scoops\n" +
                            $"{sugarDisplay}";
    }

    void Update()
    {
        if (EventManager.instance != null && EventManager.instance.isHotWeather)
        {
            if (iceScoopCount > 0 && !isTrashCup)
            {
                meltTimer -= Time.deltaTime;
                if (meltTimer <= 0f)
                {
                    Debug.Log("Ice melted in the heat! Cup ruined.");
                    RuinCup();
                    meltTimer = 5f;
                }
            }
        }
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
    }

    private void ValidateRecipe()
    {
        if (validRecipes == null)
            return;

        bool isValidCombo = false;

        foreach (DrinkRecipe recipe in validRecipes)
        {
            if (recipe.MatchesRecipe(base1, base2))
            {
                isValidCombo = true;
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
    }

    public void AddBobaParticles(int amount)
    {
        bobaParticleCount += amount;
        UpdateUI();
        OnBobaAdded?.Invoke();
    }
}
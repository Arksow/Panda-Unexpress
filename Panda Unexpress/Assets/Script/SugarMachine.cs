using UnityEngine;
using Oculus.Interaction;

public class SugarMachine : MonoBehaviour
{
    [Header("Lever Setup")]
    public Transform slotLever;
    public MetaSocket cupSocket;

    public enum SugarType { Syrup, Honey }
    public SugarType selectedType = SugarType.Syrup;
    private readonly float[] sugarLevels = { 0f, 25f, 50f, 100f };

    public ParticleSystem syrupStream;
    public ParticleSystem honeyStream;

    private float lastLeverAngle;
    private int spinFixCount = 0;

    public void ToggleSugarType()
    {
        selectedType = (selectedType == SugarType.Syrup) ? SugarType.Honey : SugarType.Syrup;
        Debug.Log("Switched to: " + selectedType);
    }

    public void SetToSyrup()
    {
        selectedType = SugarType.Syrup;
        Debug.Log("Switched to: " + selectedType);
    }

    public void SetToHoney()
    {
        selectedType = SugarType.Honey;
        Debug.Log("Switched to: " + selectedType);
    }

    private float GetCurrentLeverAngle()
    {
        return slotLever.localEulerAngles.x;
    }

    void Update()
    {
        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.SugarSpoil)
        {
            float currentAngle = GetCurrentLeverAngle();
            if (Mathf.Abs(currentAngle - lastLeverAngle) > 10f)
            {
                spinFixCount++;
                lastLeverAngle = currentAngle;

                if (spinFixCount > 20)
                {
                    spinFixCount = 0;
                    EventManager.instance.ResolveCurrentEvent();
                }
            }
        }
    }

    public float GetSelectedSugarLevel()
    {
        float currentAngle = GetCurrentLeverAngle();

        float normalizedLever = Mathf.InverseLerp(130f, 230f, currentAngle);

        int levelIndex = Mathf.RoundToInt(normalizedLever * 3f);
        levelIndex = Mathf.Clamp(levelIndex, 0, 3);

        return sugarLevels[levelIndex];
    }

    public void DispenseSugar()
    {
        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.SugarSpoil)
        {
            Debug.Log("Machine is jammed! Spin the lever!");
            return;
        }

        if (cupSocket.HasItem())
        {
            CupData cup = cupSocket.GetSocketItem();

            if (cup != null)
            {
                float levelToDispense = GetSelectedSugarLevel();

                if (selectedType == SugarType.Syrup) syrupStream.Play();
                if (selectedType == SugarType.Honey) honeyStream.Play();

                Debug.Log("Dispensed " + levelToDispense + "% " + selectedType);
            }
        }
    }
}
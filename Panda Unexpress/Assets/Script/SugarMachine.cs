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

    private float GetCurrentLeverAngle()
    {
        float angle = slotLever.localEulerAngles.x;

        if (angle > 225f && angle <= 360f)
        {
            angle -= 360f;
        }
        return angle;
    }

    void Update()
    {
        // Your jam mechanic!
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

        // Calculate the 0% to 100% based on your new -45 to 225 range
        float normalizedLever = Mathf.InverseLerp(-45f, 225f, currentAngle);

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
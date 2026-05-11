using UnityEngine;
using Oculus.Interaction;

public class SugarMachine : MonoBehaviour
{
    public Transform slotLever;
    public MetaSocket cupSocket;

    public SugarType selectedType = SugarType.Syrup;
    private readonly float[] sugarLevels = { 0f, 25f, 50f, 100f };

    public ParticleSystem syrupStream;
    public ParticleSystem honeyStream;

    public Axis rotationAxis = Axis.X;
    public enum Axis { X, Y, Z }

    public float zeroPercentAngle = -50f;
    public float hundredPercentAngle = 50f;

    public void ToggleSugarType() { selectedType = (selectedType == SugarType.Syrup) ? SugarType.Honey : SugarType.Syrup; }
    public void SetToSyrup() { selectedType = SugarType.Syrup; }
    public void SetToHoney() { selectedType = SugarType.Honey; }

    private float GetCurrentLeverAngle()
    {
        float angle = 0f;

        if (rotationAxis == Axis.X) angle = slotLever.localEulerAngles.x;
        else if (rotationAxis == Axis.Y) angle = slotLever.localEulerAngles.y;
        else if (rotationAxis == Axis.Z) angle = slotLever.localEulerAngles.z;

        if (angle > 180f) angle -= 360f;

        return angle;
    }

    public float GetSelectedSugarLevel()
    {
        float currentAngle = GetCurrentLeverAngle();

        float normalizedLever = Mathf.InverseLerp(zeroPercentAngle, hundredPercentAngle, currentAngle);

        int levelIndex = Mathf.RoundToInt(normalizedLever * 3f);
        levelIndex = Mathf.Clamp(levelIndex, 0, 3);

        float finalSugar = sugarLevels[levelIndex];

        Debug.Log($"Lever Axis: {rotationAxis} | Current Angle: {currentAngle} | Final Sugar: {finalSugar}%");

        return finalSugar;
    }

    public void DispenseSugar()
    {
        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.SugarSpoil) return;

        if (cupSocket.HasItem())
        {
            CupData cup = cupSocket.GetSocketItem();
            if (cup != null)
            {
                float levelToDispense = GetSelectedSugarLevel();

                if (selectedType == SugarType.Syrup && syrupStream != null) syrupStream.Play();
                if (selectedType == SugarType.Honey && honeyStream != null) honeyStream.Play();

                cup.sugarPercentage = levelToDispense;
                cup.currentSugarType = selectedType;
                cup.ShowUI();
            }
        }
    }
}
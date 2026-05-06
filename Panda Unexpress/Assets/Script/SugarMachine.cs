using UnityEngine;

public class SugarMachine : MonoBehaviour
{
    public HingeJoint slotLever;
    public MetaSocket cupSocket;
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

    void Update()
    {
        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.SugarSpoil)
        {
            if (Mathf.Abs(slotLever.angle - lastLeverAngle) > 10f)
            {
                spinFixCount++;
                lastLeverAngle = slotLever.angle;

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
        float currentAngle = slotLever.angle;
        float normalizedLever = Mathf.InverseLerp(-45f, 45f, currentAngle);
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

                cup.SetSugar(levelToDispense, selectedType);
            }
        }
    }
}
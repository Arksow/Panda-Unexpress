using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SugarMachine : MonoBehaviour
{
    public HingeJoint slotLever;
    public XRSocketInteractor cupSocket;
    public SugarType selectedType = SugarType.Syrup;
    private readonly float[] sugarLevels = { 0f, 25f, 50f, 100f };

    [Header("Visuals")]
    public ParticleSystem syrupStream;
    public ParticleSystem honeyStream;

    public void ToggleSugarType()
    {
        selectedType = (selectedType == SugarType.Syrup) ? SugarType.Honey : SugarType.Syrup;
        Debug.Log("Switched to: " + selectedType);
    }

    public float GetSelectedSugarLevel()
    {
        float currentAngle = slotLever.angle;
        float normalizedLever = Mathf.InverseLerp(-45f, 45f, currentAngle);
        int levelIndex = Mathf.RoundToInt(normalizedLever * 3f);
        levelIndex = Mathf.Clamp(levelIndex, 0, 3);

        return sugarLevels[levelIndex];
    }

    public  void DispenseSugar()
    {
        if (cupSocket.hasSelection)
        {
            IXRSelectInteractable cupInteractable = cupSocket.interactablesSelected[0];
            CupData cup = cupInteractable.transform.GetComponent<CupData>();

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
using UnityEngine;

public class SugarMachine : MonoBehaviour
{
    public Transform slotLever;
    public MetaSocket cupSocket;

    public SugarType selectedType = SugarType.Syrup;
    private readonly float[] sugarLevels = { 0f, 25f, 50f, 75f, 100f };

    public ParticleSystem syrupStream;
    public ParticleSystem honeyStream;

    public Axis rotationAxis = Axis.X;
    public enum Axis { X, Y, Z }

    public float zeroPercentAngle = -50f;
    public float hundredPercentAngle = 50f;

    [Header("Event Resolution")]
    public float angleTolerance = 10f;
    private int requiredSwings = 0;
    private int currentExtremeHits = 0;
    private bool lastExtremeWasZero = false;
    private bool lastExtremeWasHundred = false;

    public AudioClip brokenSparkSound;

    public void ToggleSugarType() { selectedType = (selectedType == SugarType.Syrup) ? SugarType.Honey : SugarType.Syrup; }
    public void SetToSyrup() { selectedType = SugarType.Syrup; }
    public void SetToHoney() { selectedType = SugarType.Honey; }

    private void Update()
    {
        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.SugarSpoil)
        {
            if (requiredSwings == 0)
            {
                requiredSwings = Random.Range(1, 5);
                currentExtremeHits = 0;
                lastExtremeWasZero = false;
                lastExtremeWasHundred = false;
                Debug.Log($"Sugar Machine broken! Move lever up and down {requiredSwings} times to fix.");
            }

            float currentAngle = GetCurrentLeverAngle();

            bool isAtZero = Mathf.Abs(currentAngle - zeroPercentAngle) <= angleTolerance;
            bool isAtHundred = Mathf.Abs(currentAngle - hundredPercentAngle) <= angleTolerance;

            if (isAtZero && !lastExtremeWasZero)
            {
                lastExtremeWasZero = true;
                lastExtremeWasHundred = false;
                currentExtremeHits++;
            }
            else if (isAtHundred && !lastExtremeWasHundred)
            {
                lastExtremeWasHundred = true;
                lastExtremeWasZero = false;
                currentExtremeHits++;
            }

            if (currentExtremeHits >= requiredSwings * 2)
            {
                Debug.Log("Sugar Machine fixed!");
                EventManager.instance.ResolveCurrentEvent();

                requiredSwings = 0;
                currentExtremeHits = 0;
                lastExtremeWasZero = false;
                lastExtremeWasHundred = false;
            }
        }
        else if (requiredSwings != 0)
        {
            requiredSwings = 0;
            currentExtremeHits = 0;
            lastExtremeWasZero = false;
            lastExtremeWasHundred = false;
        }
    }

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

        int levelIndex = Mathf.RoundToInt(normalizedLever * 4f);
        levelIndex = Mathf.Clamp(levelIndex, 0, 4);

        float finalSugar = sugarLevels[levelIndex];

        return finalSugar;
    }

    public void DispenseSugar()
    {
        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.SugarSpoil)
        {
            if (AudioController.Instance != null && brokenSparkSound != null)
            {
                AudioController.Instance.PlaySpatialSFX(brokenSparkSound, transform.position);
            }
            return;
        }

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

                cup.OnSugarAdded?.Invoke();
            }
        }
    }
}
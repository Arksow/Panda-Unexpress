using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using UnityEngine;

public class SugarMachine : MonoBehaviour
{
    public Transform slotLever;
    public MetaSocket cupSocket;
    public Grabbable leverGrabbable;

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
    private float lastLeverAngle = 0f;
    private float nextCreakTime = 0f;

    [Header("Audio & Timers")]
    public AudioClip brokenSparkSound;
    public AudioClip creakingSound;
    public AudioClip dispensingSound;
    public float dispenseDuration = 2f;

    private bool isDispensing = false;
    public AudioSource machineAudioSource;

    public void ToggleSugarType()
    {
        selectedType =
            (selectedType == SugarType.Syrup) ? SugarType.Honey :
            (selectedType == SugarType.Honey) ? SugarType.BrownSugar :
            SugarType.Syrup;
    }
    public void SetToSyrup() { selectedType = SugarType.Syrup; }
    public void SetToHoney() { selectedType = SugarType.Honey; }
    public void SetToBrownSugar() { selectedType = SugarType.BrownSugar; }

    private void Start()
    {
        lastLeverAngle = GetCurrentLeverAngle();
    }

    private void Update()
    {
        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.SugarSpoil)
        {
            if (requiredSwings == 0)
            {
                requiredSwings = Random.Range(1, 5) * 2;
            }

            float currentAngle = GetCurrentLeverAngle();
            float moveDistance = Mathf.Abs(Mathf.DeltaAngle(currentAngle, lastLeverAngle));

            bool isGrabbed = leverGrabbable != null && leverGrabbable.SelectingPointsCount > 0;

            if (isGrabbed && moveDistance > 1.0f)
            {
                if (Time.time > nextCreakTime)
                {
                    if (AudioController.Instance != null && creakingSound != null)
                    {
                        AudioController.Instance.PlaySpatialSFX(creakingSound, transform.position);
                    }
                    nextCreakTime = Time.time + 0.4f;
                }
            }
            lastLeverAngle = currentAngle;

            bool isAtZero = Mathf.Abs(currentAngle - zeroPercentAngle) <= angleTolerance;
            bool isAtHundred = Mathf.Abs(currentAngle - hundredPercentAngle) <= angleTolerance;

            if (isAtZero && !lastExtremeWasZero)
            {
                currentExtremeHits++;
                lastExtremeWasZero = true;
                lastExtremeWasHundred = false;
            }
            else if (isAtHundred && !lastExtremeWasHundred)
            {
                currentExtremeHits++;
                lastExtremeWasHundred = true;
                lastExtremeWasZero = false;
            }

            if (currentExtremeHits >= requiredSwings)
            {
                EventManager.instance.ResolveEvent();
                requiredSwings = 0;
                currentExtremeHits = 0;
            }
        }
    }

    private float GetCurrentLeverAngle()
    {
        float angle = 0f;
        switch (rotationAxis)
        {
            case Axis.X: angle = slotLever.localEulerAngles.x; break;
            case Axis.Y: angle = slotLever.localEulerAngles.y; break;
            case Axis.Z: angle = slotLever.localEulerAngles.z; break;
        }
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    public float GetSelectedSugarLevel()
    {
        float currentAngle = GetCurrentLeverAngle();
        float normalizedLever = Mathf.InverseLerp(zeroPercentAngle, hundredPercentAngle, currentAngle);
        int levelIndex = Mathf.RoundToInt(normalizedLever * 4f);
        levelIndex = Mathf.Clamp(levelIndex, 0, 4);
        return sugarLevels[levelIndex];
    }

    public void DispenseSugar()
    {
        if (isDispensing) return;

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
            if (cup != null && !cup.isSealed)
            {
                StartCoroutine(DispenseRoutine(cup));
            }
        }
    }

    private IEnumerator DispenseRoutine(CupData cup)
    {
        isDispensing = true;

        HandGrabInteractable[] handGrabs = cup.GetComponentsInChildren<HandGrabInteractable>();
        GrabInteractable[] controllerGrabs = cup.GetComponentsInChildren<GrabInteractable>();

        foreach (var hg in handGrabs) hg.enabled = false;
        foreach (var cg in controllerGrabs) cg.enabled = false;

        Grabbable cupGrabbable = cup.GetComponent<Grabbable>();
        if (cupGrabbable != null) cupGrabbable.enabled = false;

        if (machineAudioSource != null && dispensingSound != null)
        {
            machineAudioSource.clip = dispensingSound;
            machineAudioSource.Play();
        }

        float levelToDispense = GetSelectedSugarLevel();
        if ((selectedType == SugarType.Syrup && syrupStream != null) || (selectedType == SugarType.BrownSugar && honeyStream != null)) syrupStream.Play();
        if (selectedType == SugarType.Honey && honeyStream != null) honeyStream.Play();

        yield return new WaitForSeconds(dispenseDuration);

        if (machineAudioSource != null)
        {
            machineAudioSource.Stop();
        }

        cup.sugarPercentage = levelToDispense;
        cup.currentSugarType = selectedType;
        cup.UpdateUI();
        cup.OnSugarAdded?.Invoke();
        if (syrupStream != null) syrupStream.Stop();
        if (honeyStream != null) honeyStream.Stop();

        foreach (var hg in handGrabs) hg.enabled = true;
        foreach (var cg in controllerGrabs) cg.enabled = true;
        if (cupGrabbable != null) cupGrabbable.enabled = true;

        isDispensing = false;
    }
}
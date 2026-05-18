using UnityEngine;
using Oculus.Interaction;

public class CupDispenser : MonoBehaviour
{
    public MetaSocket dispenserSocket;
    public GameObject cupPrefab;

    public int maxCups = 10;
    private int cupsRemaining;

    public DrinkRecipe[] masterRecipeList;

    private Grabbable currentCup;
    private Rigidbody currentRb;
    public System.Action OnDispenserRefilled;

    public AudioClip popSound;

    private OVRInput.Controller vibratedController = OVRInput.Controller.None;

    private void Start()
    {
        cupsRemaining = maxCups;
        SpawnNewCup();
    }

    private void Update()
    {
        if (currentCup != null && !dispenserSocket.HasItem())
        {
            OnCupGrabbed();
        }
    }

    private void SpawnNewCup()
    {
        GameObject newCup = Instantiate(cupPrefab, dispenserSocket.attachPoint.position, dispenserSocket.attachPoint.rotation);

        currentCup = newCup.GetComponent<Grabbable>();

        CupData cupData = newCup.GetComponent<CupData>();
        if (cupData != null)
        {
            cupData.enabled = false;
            cupData.validRecipes = this.masterRecipeList;
        }

        StartCoroutine(DelayedSocket(currentCup));
    }

    private System.Collections.IEnumerator DelayedSocket(Grabbable cup)
    {
        yield return null;
        dispenserSocket.ForceSocket(cup);
    }

    private void OnCupGrabbed()
    {
        CupData cupData = currentCup.GetComponent<CupData>();
        if (cupData != null)
        {
            cupData.enabled = true;
        }

        if (AudioController.Instance != null && popSound != null)
        {
            AudioController.Instance.PlaySpatialSFX(popSound, dispenserSocket.attachPoint.position);
        }

        float leftGrab = Mathf.Max(
            OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch),
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch));

        float rightGrab = Mathf.Max(
            OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch),
            OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch));

        if (rightGrab > leftGrab && OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
        {
            OVRInput.SetControllerVibration(0.6f, 0.6f, OVRInput.Controller.RTouch);
            vibratedController = OVRInput.Controller.RTouch;
        }
        else if (leftGrab > rightGrab && OVRInput.IsControllerConnected(OVRInput.Controller.LTouch))
        {
            OVRInput.SetControllerVibration(0.6f, 0.6f, OVRInput.Controller.LTouch);
            vibratedController = OVRInput.Controller.LTouch;
        }

        if (vibratedController != OVRInput.Controller.None)
        {
            Invoke(nameof(StopHaptics), 0.1f);
        }

        currentCup = null;
        cupsRemaining--;

        if (cupsRemaining > 0)
        {
            StartCoroutine(WaitAndSpawnCup());
        }
    }

    private System.Collections.IEnumerator WaitAndSpawnCup()
    {
        yield return new WaitForSeconds(0.75f);
        yield return new WaitUntil(() => !dispenserSocket.IsBlocked());
        SpawnNewCup();
    }

    private void StopHaptics()
    {
        if (vibratedController != OVRInput.Controller.None)
        {
            OVRInput.SetControllerVibration(0, 0, vibratedController);
            vibratedController = OVRInput.Controller.None;
        }
    }

    public void RefillDispenser()
    {
        cupsRemaining = maxCups;
        if (currentCup == null)
        {
            SpawnNewCup();
        }
        Debug.Log("Dispenser Refilled via Voice Command!");

        OnDispenserRefilled?.Invoke();
    }
}
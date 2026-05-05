using UnityEngine;
using Oculus.Interaction;

public class CupDispenser : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject cupPrefab;

    public int maxCups = 10;
    private int cupsRemaining;

    private Grabbable currentCup;
    private Rigidbody currentRb;

    private void Start()
    {
        cupsRemaining = maxCups;
        SpawnNewCup();
    }

    private void Update()
    {
        if (currentCup != null && currentCup.SelectingPointsCount > 0)
        {
            OnCupGrabbed();
        }
    }

    private void SpawnNewCup()
    {
        GameObject newCup = Instantiate(cupPrefab, spawnPoint.position, spawnPoint.rotation);

        currentCup = newCup.GetComponent<Grabbable>();
        currentRb = newCup.GetComponent<Rigidbody>();

        if (currentRb != null)
        {
            currentRb.isKinematic = true;
        }

        CupData cupData = newCup.GetComponent<CupData>();
        if (cupData != null)
        {
            cupData.enabled = false;
        }
    }

    private void OnCupGrabbed()
    {
        if (currentRb != null)
        {
            currentRb.isKinematic = false;
        }

        CupData cupData = currentCup.GetComponent<CupData>();
        if (cupData != null)
        {
            cupData.enabled = true;
        }

        currentCup = null;
        cupsRemaining--;

        if (cupsRemaining > 0)
        {
            SpawnNewCup();
        }
        else
        {
            Debug.Log("Dispenser empty! Say 'refill'...");
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
    }
}
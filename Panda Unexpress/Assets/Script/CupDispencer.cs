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

        dispenserSocket.ForceSocket(currentCup);
    }

    private void OnCupGrabbed()
    {
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
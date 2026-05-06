using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [System.Serializable]
    public class CustomerSlot
    {
        public Transform position;
        public OrderUIManager slotUI;
        public bool isOccupied;
    }

    [Header("Spawn Settings")]
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;

    [Header("Locations")]
    public Transform leaveLocation;

    [Header("Order System")]
    public OrderSystem orderSystem;

    [Header("Counter Slots")]
    public CustomerSlot[] counterSlots;

    [Header("Progression")]
    private float spawnInterval = 1f;
    private float waveDelay = 5f;

    [Header("End Game")]
    private int maxFailedOrders = 3;
    private int failedOrders = 0;
    private bool isGameOver = false;

    private int nextID = 1;
    private int currentWave = 1;
    private int activeCustomers = 0;

    private int customersThisWave = 1;
    private int customersWithExtraDrink = 0;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (!isGameOver)
        {
            // Prepare which customers get extra drinks
            List<int> extraDrinkIndexes = new List<int>();

            while (extraDrinkIndexes.Count < customersWithExtraDrink)
            {
                int randomIndex = Random.Range(0, customersThisWave);

                if (!extraDrinkIndexes.Contains(randomIndex))
                {
                    extraDrinkIndexes.Add(randomIndex);
                }
            }

            // Spawn customers
            for (int i = 0; i < customersThisWave; i++)
            {
                if (isGameOver) yield break;

                while (!HasFreeSlot())
                {
                    yield return null;
                }

                bool giveExtraDrink = extraDrinkIndexes.Contains(i);
                SpawnCustomer(giveExtraDrink);

                yield return new WaitForSeconds(spawnInterval);
            }

            // Wait until all customers leave
            while (activeCustomers > 0)
            {
                if (isGameOver) yield break;
                yield return null;
            }

            currentWave++;
            ApplyRandomModifier();

            yield return new WaitForSeconds(waveDelay);
        }
    }

    void ApplyRandomModifier()
    {
        int modifiersToApply = currentWave - 1;
        customersWithExtraDrink = 0;

        for (int i = 0; i < modifiersToApply; i++)
        {
            int randomModifier = Random.Range(0, 2);

            if (randomModifier == 0)
            {
                customersThisWave++; // +1 customer
            }
            else
            {
                customersWithExtraDrink++;

                customersWithExtraDrink = Mathf.Min(customersWithExtraDrink, customersThisWave);
            }
        }
    }

    bool HasFreeSlot()
    {
        foreach (var slot in counterSlots)
        {
            if (!slot.isOccupied)
                return true;
        }
        return false;
    }

    void SpawnCustomer(bool extraDrink)
    {
        CustomerSlot freeSlot = GetFreeSlot();

        if (freeSlot == null)
            return;

        GameObject randomCustomer = customerPrefabs[Random.Range(0, customerPrefabs.Length)];

        GameObject customer = Instantiate(
            randomCustomer,
            spawnPoint.position,
            spawnPoint.rotation
        );

        CustomerAI ai = customer.GetComponent<CustomerAI>();

        ai.gameSystem = this;
        ai.customerID = nextID++;
        ai.customerLocation = freeSlot.position;
        ai.leaveLocation = leaveLocation;
        ai.orderGenerator = orderSystem;
        ai.orderCount = extraDrink ? 2 : 1;
        ai.orderUI = freeSlot.slotUI;
        freeSlot.slotUI.SetCustomer(ai);
        freeSlot.isOccupied = true;
        activeCustomers++;

        ai.onCustomerLeave += () =>
        {
            freeSlot.isOccupied = false;
            activeCustomers--;
            freeSlot.slotUI.ClearCustomer(ai);
        };
    }

    CustomerSlot GetFreeSlot()
    {
        foreach (var slot in counterSlots)
        {
            if (!slot.isOccupied)
                return slot;
        }
        return null;
    }

    public void RegisterFailedOrder()
    {
        failedOrders++;

        if (failedOrders >= maxFailedOrders)
        {
            isGameOver = true;
            Debug.Log("Game Over!");

            StopAllCoroutines();
        }
    }
}
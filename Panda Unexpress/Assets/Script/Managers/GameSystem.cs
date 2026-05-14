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
    private float waveDelay = 20f;

    [Header("Wave UI")]
    public GameObject wavePanel;
    public TextMeshProUGUI waveText;

    [Header("End Game")]
    public GameOverUIManager gameOverUI;
    public int maxFailedOrders = 3;
    public int failedOrders = 0;
    private bool isGameOver = false;

    private int nextID = 1;
    public int currentWave = 1;
    public int activeCustomers = 0;

    private int customerStartingWave = 5;
    private int customersWithExtraDrink = 0;

    private bool waitingForNextWave = false;

    void Start()
    {
        if (wavePanel != null)
            wavePanel.SetActive(false);

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
                int randomIndex = Random.Range(0, customerStartingWave);

                if (!extraDrinkIndexes.Contains(randomIndex))
                {
                    extraDrinkIndexes.Add(randomIndex);
                }
            }

            // Spawn customers
            for (int i = 0; i < customerStartingWave; i++)
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

            ShowWaveUI();

            // Wait for player OR timeout
            waitingForNextWave = true;
            float timer = 0f;

            while (waitingForNextWave)
            {
                if (isGameOver) yield break;

                timer += Time.deltaTime;

                if (waveText != null)
                {
                    int secondsLeft = Mathf.CeilToInt(waveDelay - timer);
                    secondsLeft = Mathf.Max(0, secondsLeft);
                    waveText.text = $"Wave {currentWave} Complete!\nNext in {secondsLeft}s";
                }

                // Auto next wave
                if (timer >= waveDelay)
                {
                    waitingForNextWave = false;
                }

                yield return null;
            }

            // Hide UI
            if (wavePanel != null)
                wavePanel.SetActive(false);

            // Move to next wave
            currentWave++;
            ApplyRandomModifier();
        }
    }

    void ShowWaveUI()
    {
        if (wavePanel != null)
        {
            wavePanel.SetActive(true);
        }
    }

    public void NextWaveButton()
    {
        waitingForNextWave = false;

        if (wavePanel != null)
            wavePanel.SetActive(false);
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
                customerStartingWave++;
            }
            else
            {
                customersWithExtraDrink++;
                customersWithExtraDrink = Mathf.Min(customersWithExtraDrink, customerStartingWave);
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

        ai.decreaseSpeed = 0.001f + ((currentWave - 1) * 0.0002f);

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
        if (isGameOver) return;

        failedOrders++;

        if (failedOrders >= maxFailedOrders)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("Shift Over! Too many angry customers.");

        CustomerAI[] allCustomers = FindObjectsByType<CustomerAI>(FindObjectsSortMode.None);

        foreach (CustomerAI ai in allCustomers)
        {
            ai.enabled = false;

            UnityEngine.AI.NavMeshAgent agent = ai.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.isStopped = true;
            }

            if (ai.animator != null)
            {
                ai.animator.speed = 0f;
            }
        }

        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOverUI();
        }
    }
}
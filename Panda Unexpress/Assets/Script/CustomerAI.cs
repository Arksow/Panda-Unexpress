using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerAI : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject customerPrefab;
    public Transform spawnPoint;

    [Header("Locations")]
    public Transform leaveLocation;
    public Transform firstLocation;

    [Header("Progress Settings")]
    private float decreaseSpeed = 0.01f;

    [Header("Order System")]
    private bool orderGenerated = false;
    public TextMeshProUGUI orderText;

    private GameObject spawnedCustomer;
    private NavMeshAgent agent;
    private Image progressImage;

    private bool reachedCounter = false;
    private bool isLeaving = false;

    public CustomerOrder currentOrder;
    public OrderSystem orderGenerator;
    public CupData receivedOrder;

    void Start()
    {
        SpawnCustomer();
    }

    void Update()
    {
        if (agent == null) return;

        //Move to the first location
        if (!reachedCounter &&
            !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            reachedCounter = true;
            agent.isStopped = true; //Stop the agent to rotate in place
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.updateRotation = false;
        }

        //Customer rotates to zero and decrease progress bar until it reaches zero, then leave the store
        if (reachedCounter && !isLeaving)
        {
            RotateToZero();

            if (!orderGenerated)
            {
                //CustomerOrderSetup();
                currentOrder = orderGenerator.GenerateOrder();
                orderGenerated = true;
            }

            progressImage.fillAmount -= decreaseSpeed * Time.deltaTime;

            if (progressImage.fillAmount <= 0f)
            {
                progressImage.fillAmount = 0f;
                CustomerLeave();
            }
        }

        //Leave the store and destroy when reached leave location
        if (isLeaving &&
            !agent.pathPending &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            Destroy(spawnedCustomer);
        }
    }

    void SpawnCustomer()
    {
        spawnedCustomer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        agent = spawnedCustomer.GetComponent<NavMeshAgent>();

        progressImage = spawnedCustomer.GetComponentInChildren<Image>();

        if (progressImage != null)
            progressImage.fillAmount = 1f;

        if (agent != null)
            agent.SetDestination(firstLocation.position);
    }

    void RotateToZero()
    {
        Quaternion targetRotation = firstLocation.rotation;

        spawnedCustomer.transform.rotation = Quaternion.Slerp(
            spawnedCustomer.transform.rotation,
            targetRotation,
            5f * Time.deltaTime
        );
    }

    void CustomerLeave()
    {
        isLeaving = true;
        agent.isStopped = false;
        agent.SetDestination(leaveLocation.position);
    }

    //void CustomerOrderSetup()
    //{
    //    currentOrder = orderGenerator.GenerateOrder();

    //    string iceText = GetIceText(currentOrder.iceAmount);

    //    orderText.text =
    //        "Customer Order:\n" +
    //        currentOrder.base1 + " + " + currentOrder.base2 + "\n" +
    //        currentOrder.sugarType + " " + currentOrder.sugarPercent + "%\n" +
    //        iceText + "\n" +
    //        "Boba: " + (currentOrder.wantsBoba ? "Yes" : "No");
    //}
    string GetIceText(int iceAmount)
    {
        switch (iceAmount)
        {
            case 0:
                return "No Ice";
            case 1:
                return "Less Ice";
            case 2:
                return "Regular Ice";
            case 3:
                return "More Ice";
            default:
                return "Unknown";
        }
    }
    
    public void CheckOrder()
    {
        if (currentOrder == null || receivedOrder == null)
            return;

        if (currentOrder.sugarPercent != receivedOrder.sugarPercentage ||
            currentOrder.sugarType != receivedOrder.currentSugarType ||
            currentOrder.iceAmount != receivedOrder.iceScoopCount ||
            //currentOrder.wantsBoba != (receivedOrder.bobaParticleCount > 0) ||
            currentOrder.base1 != receivedOrder.base1 ||
            currentOrder.base2 != receivedOrder.base2)
        {
            Debug.Log("Customer is unhappy with the order!");
        }
        else
        {
            Debug.Log("Customer is happy with the order!");
        }

        progressImage.fillAmount = 0f;
        CustomerLeave();
    }
}
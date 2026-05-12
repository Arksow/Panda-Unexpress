using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CustomerAI : MonoBehaviour
{
    [Header("Locations")]
    [HideInInspector] public Transform leaveLocation;
    [HideInInspector] public Transform customerLocation;

    [Header("Order System")]
    [HideInInspector] public OrderSystem orderGenerator;

    [Header("Customer Info")]
    public int customerID;
    private bool hasFailed = false;

    [Header("Animations")]
    public Animator animator;

    public List<CustomerOrder> currentOrders = new List<CustomerOrder>();
    [HideInInspector] public int orderCount = 1;
    public CupData receivedOrder;

    [Header("UI")]
    [HideInInspector] public OrderUIManager orderUI;

    [Header("Progress")]
    private float decreaseSpeed = 0.001f;
    private bool orderGenerated = false;

    private NavMeshAgent agent;
    [SerializeField] private Image progressImage;

    private bool reachedCounter = false;
    private bool isLeaving = false;

    public System.Action onCustomerLeave;
    public GameSystem gameSystem;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator.SetBool("Walking", true);

        if (progressImage != null)
            progressImage.fillAmount = 1f;

        if (customerLocation != null)
            agent.SetDestination(customerLocation.position);
    }

    void Update()
    {
        if (agent == null) return;

        // ARRIVE AT COUNTER
        if (!reachedCounter &&
            !agent.pathPending &&
            agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            reachedCounter = true;
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.updateRotation = false;
            animator.SetBool("Walking", false);
            animator.SetTrigger("Ordering");
        }

        // WAITING STATE
        if (reachedCounter && !isLeaving)
        {
            RotateToCounter();

            if (!orderGenerated)
            {
                SetupOrder();
                orderGenerated = true;
            }

            progressImage.fillAmount -= decreaseSpeed * Time.deltaTime;

            if (progressImage.fillAmount <= 0f && !hasFailed)
            {
                hasFailed = true;
                gameSystem?.RegisterFailedOrder();
                animator.SetTrigger("Angry");
                StartCoroutine(LeaveAfterAngry());
            }
        }

        // DESTROY AFTER LEAVING
        if (isLeaving &&
            !agent.pathPending &&
            agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            onCustomerLeave?.Invoke();
            Destroy(gameObject);
        }
    }

    void RotateToCounter()
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            customerLocation.rotation,
            5f * Time.deltaTime
        );
    }

    void SetupOrder()
    {
        currentOrders.Clear();

        for (int i = 0; i < orderCount; i++)
        {
            CustomerOrder order = orderGenerator.GenerateOrder();
            currentOrders.Add(order);
        }

        orderUI?.SetCustomer(this);
    }

    void LeaveStore()
    {
        ClearCustomerUI();
        animator.SetBool("Walking", true);
        isLeaving = true;
        agent.isStopped = false;
        agent.updateRotation = true;
        agent.SetDestination(leaveLocation.position);
    }

    public void CheckOrder(CupData cup)
    {
        receivedOrder = cup;

        if (currentOrders.Count == 0)
        {
            Debug.Log("No more orders for this customer");
            return;
        }

        CustomerOrder currentOrder = currentOrders[0];

        bool correct =
            currentOrder.sugarPercent == receivedOrder.sugarPercentage &&
            currentOrder.sugarType == receivedOrder.currentSugarType &&
            currentOrder.iceAmount == receivedOrder.iceScoopCount &&
            currentOrder.base1 == receivedOrder.base1 &&
            currentOrder.base2 == receivedOrder.base2;

        if (correct)
        {
            Debug.Log($"Customer {customerID} order completed!");

            currentOrders.RemoveAt(0);

            orderUI?.UpdateUI();

            if (currentOrders.Count == 0)
            {
                Debug.Log($"Customer {customerID} is HAPPY and leaving!");
                progressImage.fillAmount = 0f;

                ClearCustomerUI();
                LeaveStore();
            }
        }
        else
        {
            Debug.Log($"Customer {customerID} is UNHAPPY!");

            if (!hasFailed)
            {
                hasFailed = true;
                gameSystem?.RegisterFailedOrder();
            }

            StartCoroutine(LeaveAfterAngry());
            ClearCustomerUI();
            LeaveStore();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cup"))
        {
            CupData cup = other.GetComponent<CupData>();

            if (cup != null)
            {
                receivedOrder = cup;
                CheckOrder(cup);
                Destroy(other.gameObject);
            }
        }
    }

    public void SetPatienceMultiplier(float multiplier)
    {
        decreaseSpeed *= multiplier;
    }

    void ClearCustomerUI()
    {
        if (orderUI != null)
        {
            orderUI.ClearCustomer(this);
        }
    }

    IEnumerator LeaveAfterAngry()
    {
        yield return new WaitForSeconds(2f);
        LeaveStore();
    }
}
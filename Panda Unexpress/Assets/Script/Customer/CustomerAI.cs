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
    [HideInInspector]
    public float decreaseSpeed = 0.01f;
    private bool orderGenerated = false;

    [Header("Speech Bubble")]
    [SerializeField] private GameObject textBubble;
    [SerializeField] private GameObject heartBubble;
    private float bubbleDuration = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip correctOrder;
    [SerializeField] private AudioClip wrongOrder;
    [SerializeField] private AudioClip hurryUp;

    private NavMeshAgent agent;
    [SerializeField] private Image progressImage;

    private bool reachedCounter = false;
    private bool isLeaving = false;
    private bool hasHurried = false;

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

        if (textBubble != null)
            textBubble.SetActive(false);

        if (heartBubble != null)
            heartBubble.SetActive(false);
    }

    void Update()
    {
        if (agent == null) return;

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
            StartCoroutine(ShowTextBubble());
        }

        if (reachedCounter && !isLeaving)
        {
            RotateToCounter();

            if (!orderGenerated)
            {
                SetupOrder();
                orderGenerated = true;
            }

            progressImage.fillAmount -= decreaseSpeed * Time.deltaTime;

            if (progressImage.fillAmount <= 30f && !hasHurried)
            {
                AudioController.Instance?.PlayGlobalSFX(hurryUp);
                hasHurried = true;
            }

            if (progressImage.fillAmount <= 0f && !hasFailed)
            {
                hasFailed = true;
                gameSystem?.RegisterFailedOrder();
                StartCoroutine(LeaveAfterAngry());
            }
        }

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

    int GetUpgradeBonus(CupData cup)
    {
        int bonus = 0;
        bool chocolateBase =
            cup.base1.ToString().ToLower().Contains("chocolate") ||
            cup.base2.ToString().ToLower().Contains("chocolate");
        bool extraAloe = cup.aloeScoopCount > 1;
        bool brownSugar =
            cup.currentSugarType.ToString().ToLower().Contains("brownsugar");
        if (chocolateBase) bonus += 2;
        if (extraAloe) bonus += 2;
        if (brownSugar) bonus += 2;
        return bonus;
    }

    public void CheckOrder(CupData cup)
    {
        if (isLeaving) return;

        receivedOrder = cup;

        if (currentOrders.Count == 0) return;

        int matchedIndex = -1;

        for (int i = 0; i < currentOrders.Count; i++)
        {
            CustomerOrder currentOrder = currentOrders[i];

            bool baseMatch =
                (currentOrder.base1 == receivedOrder.base1 &&
                 currentOrder.base2 == receivedOrder.base2) ||

                (currentOrder.base1 == receivedOrder.base2 &&
                 currentOrder.base2 == receivedOrder.base1);

            bool correct =
                currentOrder.sugarPercent == receivedOrder.sugarPercentage &&
                currentOrder.sugarType == receivedOrder.currentSugarType &&
                currentOrder.iceAmount == receivedOrder.iceScoopCount &&
                currentOrder.bobaAmount == receivedOrder.bobaScoopCount &&
                currentOrder.aloeAmount == receivedOrder.aloeScoopCount &&
                baseMatch &&
                !receivedOrder.isTrashCup &&
                receivedOrder.isSealed;

            if (correct)
            {
                matchedIndex = i;
                break;
            }
        }

        if (matchedIndex != -1)
        {
            currentOrders.RemoveAt(matchedIndex);

            if (EconomyManager.instance != null)
            {
                StartCoroutine(ShowHeartBubble());
                int reward = 10 + GetUpgradeBonus(receivedOrder);
                EconomyManager.instance.AddMoney(reward);
            }

            if (EventManager.instance != null)
            {
                EventManager.instance.RollForRandomEvent();
            }

            orderUI?.UpdateUI();

            if (currentOrders.Count == 0)
            {
                progressImage.fillAmount = 0f;
                AudioController.Instance?.PlayGlobalSFX(correctOrder);
                ClearCustomerUI();
                LeaveStore();
            }
        }
        else
        {
            if (!hasFailed)
            {
                hasFailed = true;
                gameSystem?.RegisterFailedOrder();
                AudioController.Instance?.PlayGlobalSFX(wrongOrder);
            }

            StartCoroutine(LeaveAfterAngry());
            ClearCustomerUI();
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

    void ClearCustomerUI()
    {
        if (orderUI != null)
        {
            orderUI.ClearCustomer(this);
        }
    }

    IEnumerator LeaveAfterAngry()
    {
        animator.SetTrigger("Angry");
        yield return new WaitForSeconds(3f);
        LeaveStore();
    }

    IEnumerator ShowTextBubble()
    {
        textBubble.SetActive(true);
        yield return new WaitForSeconds(bubbleDuration);
        textBubble.SetActive(false);
    }

    IEnumerator ShowHeartBubble()
    {
        heartBubble.SetActive(true);
        yield return new WaitForSeconds(bubbleDuration);
        heartBubble.SetActive(false);
    }
}
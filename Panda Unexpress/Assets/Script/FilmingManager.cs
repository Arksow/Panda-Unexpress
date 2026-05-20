using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FilmingManager : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;
    public float spawnDelay = 3f;

    [Header("Single Slot")]
    public Transform waitingSlot;
    public Transform exitPoint;

    [Header("Timing")]
    public float waitTime = 5f;

    private List<CustomerData> customers = new List<CustomerData>();
    private List<GameObject> prefabBag = new List<GameObject>();

    class CustomerData
    {
        public GameObject obj;
        public NavMeshAgent agent;
        public Animator animator;

        public Transform target;

        public bool startedSequence = false;
        public bool leaving = false;
    }

    void Start()
    {
        RefillBag();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (customers.Count < 1)
                SpawnCustomer();

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void RefillBag()
    {
        prefabBag.Clear();

        foreach (GameObject prefab in customerPrefabs)
            prefabBag.Add(prefab);

        for (int i = 0; i < prefabBag.Count; i++)
        {
            GameObject temp = prefabBag[i];
            int rand = Random.Range(i, prefabBag.Count);
            prefabBag[i] = prefabBag[rand];
            prefabBag[rand] = temp;
        }
    }

    void SpawnCustomer()
    {
        if (prefabBag.Count == 0)
            RefillBag();

        GameObject prefab = prefabBag[0];
        prefabBag.RemoveAt(0);

        GameObject obj = Instantiate(
            prefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        CustomerData c = new CustomerData();
        c.obj = obj;
        c.agent = obj.GetComponent<NavMeshAgent>();
        c.animator = obj.GetComponentInChildren<Animator>();

        c.animator?.SetBool("Walking", true);

        customers.Add(c);

        MoveToSlot(c);
    }

    void MoveToSlot(CustomerData c)
    {
        c.target = waitingSlot;

        c.agent.isStopped = false;
        c.animator?.SetBool("Walking", true);

        c.agent.SetDestination(waitingSlot.position);
    }

    void Update()
    {
        for (int i = customers.Count - 1; i >= 0; i--)
        {
            CustomerData c = customers[i];

            if (c.obj == null || c.target == null)
                continue;

            if (!c.agent.pathPending &&
                c.agent.hasPath &&
                c.agent.remainingDistance <= c.agent.stoppingDistance + 0.1f)
            {
                c.agent.isStopped = true;

                // ? FIXED FACING DIRECTION (towards slot)
                Vector3 dir = waitingSlot.position - c.obj.transform.position;
                dir.y = 0f;

                if (dir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(dir);

                    c.obj.transform.rotation = Quaternion.Slerp(
                        c.obj.transform.rotation,
                        targetRot,
                        5f * Time.deltaTime
                    );
                }

                c.animator?.SetBool("Walking", false);

                if (!c.startedSequence && c.target == waitingSlot)
                {
                    c.startedSequence = true;
                    StartCoroutine(Sequence(c));
                }

                if (c.leaving)
                {
                    customers.RemoveAt(i);
                    Destroy(c.obj);
                }
            }
        }
    }

    IEnumerator Sequence(CustomerData c)
    {
        c.animator?.SetTrigger("Wave");

        yield return new WaitForSeconds(waitTime);

        c.leaving = true;

        c.agent.isStopped = false;
        c.animator?.SetBool("Walking", true);

        c.target = exitPoint;
        c.agent.SetDestination(exitPoint.position);
    }
}
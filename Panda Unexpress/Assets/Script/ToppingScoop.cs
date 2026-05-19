using UnityEngine;

public class ToppingScoop : MonoBehaviour
{
    public GameObject bobaVisual;
    public GameObject aloeVisual;

    [Header("Drop Settings")]
    [Tooltip("The prefab dropped when tipping boba")]
    public GameObject bobaDropPrefab;
    [Tooltip("The prefab dropped when tipping aloe")]
    public GameObject aloeDropPrefab;
    public Transform dropSpawnPoint;
    public float dropAngleThreshold = 0.2f;

    private ToppingType currentTopping = ToppingType.None;

    void Start()
    {
        ClearScoop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentTopping == ToppingType.None)
        {
            ToppingBin bin = other.GetComponent<ToppingBin>();
            if (bin != null)
            {
                FillScoop(bin.toppingType);
            }
        }
    }

    void Update()
    {
        if (currentTopping != ToppingType.None)
        {
            if (Vector3.Dot(transform.up, Vector3.up) < dropAngleThreshold)
            {
                DropTopping();
            }
        }
    }

    private void FillScoop(ToppingType type)
    {
        currentTopping = type;
        if (type == ToppingType.Boba && bobaVisual != null) bobaVisual.SetActive(true);
        if (type == ToppingType.AloeVera && aloeVisual != null) aloeVisual.SetActive(true);
    }

    private void DropTopping()
    {
        GameObject prefabToDrop = (currentTopping == ToppingType.Boba) ? bobaDropPrefab : aloeDropPrefab;

        if (prefabToDrop != null && dropSpawnPoint != null)
        {
            Instantiate(prefabToDrop, dropSpawnPoint.position, Quaternion.identity);
        }

        ClearScoop();
    }

    private void ClearScoop()
    {
        currentTopping = ToppingType.None;
        if (bobaVisual != null) bobaVisual.SetActive(false);
        if (aloeVisual != null) aloeVisual.SetActive(false);
    }
}
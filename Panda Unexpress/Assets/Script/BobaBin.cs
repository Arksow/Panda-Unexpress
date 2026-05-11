using UnityEngine;
using System.Collections.Generic;

public class BobaBin : MonoBehaviour
{
    public GameObject bobaPrefab;
    public Transform spawnPoint;
    public int maxBobaInBin = 5;

    private List<GameObject> activeBobaList = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("CheckAndRefillBin", 1f, 1f);
    }

    void CheckAndRefillBin()
    {
        activeBobaList.RemoveAll(item => item == null);

        if (activeBobaList.Count < maxBobaInBin)
        {
            SpawnBoba();
        }
    }

    public void SpawnBoba()
    {
        GameObject newBoba = Instantiate(bobaPrefab, spawnPoint.position, Quaternion.identity);
        newBoba.transform.SetParent(transform);

        activeBobaList.Add(newBoba);
    }
}
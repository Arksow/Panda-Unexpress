using System.Collections;
using UnityEngine;

public class FilmingManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabsToSpawn;
    public Transform spawnPoint;

    [Header("Timing")]
    public float destroyDelay = 8f;

    private int currentIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        while (currentIndex < prefabsToSpawn.Length)
        {
            GameObject spawnedObj = Instantiate(
                prefabsToSpawn[currentIndex],
                spawnPoint.position,
                spawnPoint.rotation
            );

            // Find Animator anywhere in children
            Animator anim = spawnedObj.GetComponentInChildren<Animator>();

            if (anim != null)
            {
                anim.SetTrigger("Angry");
            }

            yield return new WaitForSeconds(destroyDelay);

            Destroy(spawnedObj);

            currentIndex++;
        }
    }
}
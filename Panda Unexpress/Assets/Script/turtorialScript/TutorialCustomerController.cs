using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCustomerController : MonoBehaviour
{
    public GameSystem gameSystem;
    public string mainMenuSceneName = "MainMenu";

    private CustomerAI tutorialCustomer;
    public TutorialManager tutorialManager;  
    public int step;
    //void Start()
    //{
    //    SpawnSingleCustomer();
    //}
    public void TrySpawnCustomer(int currentStep)
    {
        if (currentStep != step) return;

        SpawnSingleCustomer();
    }
    public void SpawnSingleCustomer()
    {
        //spawn single customer ONLY
        gameSystem.StopAllCoroutines();

        var slot = gameSystem.counterSlots[0];

        GameObject prefab = gameSystem.customerPrefabs[0];

        GameObject customerObj = Instantiate(
            prefab,
            gameSystem.spawnPoint.position,
            gameSystem.spawnPoint.rotation
        );

        tutorialCustomer = customerObj.GetComponent<CustomerAI>();

        tutorialCustomer.gameSystem = gameSystem;
        tutorialCustomer.customerID = 999;
        tutorialCustomer.customerLocation = slot.position;
        tutorialCustomer.leaveLocation = gameSystem.leaveLocation;
        tutorialCustomer.orderGenerator = gameSystem.orderSystem;
        tutorialCustomer.orderCount = 1;
        tutorialCustomer.orderUI = slot.slotUI;

        slot.isOccupied = true;

        tutorialCustomer.onCustomerLeave += OnTutorialCustomerFinished;
    }

    //void OnTutorialCustomerFinished()
    //{
    //    Debug.Log("Tutorial completed. Returning to menu...");
    //    SceneManager.LoadScene(mainMenuSceneName);
    //}
    public void OnTutorialCustomerFinished()
    {
        StartCoroutine(FinishTutorialFlow());
    }

    IEnumerator FinishTutorialFlow()
    {
        //return after 5 seconds
        if (tutorialManager != null)
        {
            tutorialManager.CompleteStep(step);
        }

       
        yield return new WaitForSeconds(5f);

    
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

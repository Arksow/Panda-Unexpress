using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CupDispenser : MonoBehaviour
{
    public XRSocketInteractor cupSocket;
    public XRGrabInteractable dummyCupPrefab;
    public XRGrabInteractable realCupPrefab;
    public XRInteractionManager interactManager;

    private XRGrabInteractable currentDummy;

    private void Start()
    {
        SpawnNewDummy();
    }

    private void OnEnable()
    {
        cupSocket.selectExited.AddListener(OnCupRemoved);
    }

    private void OnDisable()
    {
        cupSocket.selectExited.RemoveListener(OnCupRemoved);
    }

    private void SpawnNewDummy()
    {
        currentDummy = Instantiate(dummyCupPrefab, cupSocket.transform.position, cupSocket.transform.rotation);
        interactManager.SelectEnter((IXRSelectInteractor)cupSocket, (IXRSelectInteractable)currentDummy);
    }

    private void OnCupRemoved(SelectExitEventArgs args)
    {
        IXRSelectInteractor playersHand = args.interactorObject;
        Destroy(currentDummy.gameObject);
        XRGrabInteractable realCup = Instantiate(realCupPrefab, playersHand.transform.position, playersHand.transform.rotation);
        interactManager.SelectEnter(playersHand, (IXRSelectInteractable)realCup);
        SpawnNewDummy();
    }
}
using Oculus.Interaction;
using UnityEngine;

public class CupSealer : MonoBehaviour
{
    [Header("References")]
    public MetaSocket cupSocket;
    public Animator sealerAnimator;

    [Header("Animation")]
    public string sealAnimationTrigger = "Seal";
    public float animationDuration = 2f;

    private bool isSealing = false;

    private void Update()
    {
        if (!isSealing && cupSocket.HasItem())
        {
            StartSealing();
        }
    }

    private void StartSealing()
    {
        CupData cup = cupSocket.GetSocketItem();
        if (cup == null || cup.isTrashCup || cup.isSealed) return;

        isSealing = true;

        Grabbable grabbable = cup.GetComponent<Grabbable>();
        if (grabbable != null) grabbable.enabled = false;

        sealerAnimator.SetTrigger(sealAnimationTrigger);
        StartCoroutine(FinishSealing(cup));
    }

    private System.Collections.IEnumerator FinishSealing(CupData cup)
    {
        yield return new WaitForSeconds(animationDuration);

        BoxCollider floorCollider = cup.GetComponentInChildren<BoxCollider>();
        if (floorCollider != null) floorCollider.enabled = false;

        if (cup.seal != null) cup.seal.SetActive(true);
        cup.isSealed = true;

        Grabbable grabbable = cup.GetComponent<Grabbable>();
        if (grabbable != null) grabbable.enabled = true;

        isSealing = false;
    }
}
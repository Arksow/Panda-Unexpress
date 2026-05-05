using UnityEngine;
public class CupSocket : MonoBehaviour
{
    public CupData currentCup { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        CupData cup = other.GetComponent<CupData>();
        if (cup != null && currentCup == null)
        {
            currentCup = cup;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CupData cup = other.GetComponent<CupData>();
        if (cup != null && currentCup == cup)
        {
            currentCup = null;
        }
    }

    public bool HasCup() => currentCup != null;
}
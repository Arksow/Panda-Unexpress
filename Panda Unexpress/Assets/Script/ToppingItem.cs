using UnityEngine;

public class ToppingItem : MonoBehaviour
{
    public ToppingType toppingType;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
}
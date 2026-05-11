using UnityEngine;
using Oculus.Interaction;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Grabbable))]
public class Phyics : MonoBehaviour
{
    private Rigidbody rb;
    private Grabbable grabbable;

    public bool isSocketed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grabbable = GetComponent<Grabbable>();
    }

    void Update()
    {
        if (isSocketed) return;

        if (grabbable.SelectingPointsCount == 0 && rb.isKinematic)
        {
            rb.isKinematic = false;
        }
    }
}
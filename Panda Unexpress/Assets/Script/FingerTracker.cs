using UnityEngine;

public class FingerTracker : MonoBehaviour
{
    public Transform targetFinger;

    void Update()
    {
        if (targetFinger != null)
        {
            transform.position = targetFinger.position;
        }
    }
}
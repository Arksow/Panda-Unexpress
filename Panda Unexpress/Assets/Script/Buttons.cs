using UnityEngine;
using UnityEngine.Events;

public class Buttons : MonoBehaviour
{
    public Transform buttonCap;
    public float pressDownDistance = 0.02f;

    public UnityEvent onButtonPressed;

    private Vector3 originalPosition;
    private bool isPressed = false;

    void Start()
    {
        if (buttonCap != null)
        {
            originalPosition = buttonCap.localPosition;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isPressed && other.CompareTag("PlayerFinger"))
        {
            isPressed = true;
            buttonCap.localPosition = originalPosition - new Vector3(0, 0, pressDownDistance);
            onButtonPressed.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isPressed && other.CompareTag("PlayerFinger"))
        {
            isPressed = false;
            buttonCap.localPosition = originalPosition;
        }
    }
}
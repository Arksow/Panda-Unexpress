using UnityEngine;
using UnityEngine.Events;

public class Buttons : MonoBehaviour
{
    public Transform buttonCap;
    public float pressDownDistance = 0.02f;

    [Header("Button Events")]
    public UnityEvent onButtonDown;
    public UnityEvent onButtonHeld;
    public UnityEvent onButtonUp;

    private Vector3 originalPosition;
    private bool isPressed = false;

    private float lastTouchTime = 0f;
    private float releaseTolerance = 0.15f;

    void Start()
    {
        if (buttonCap != null)
        {
            originalPosition = buttonCap.localPosition;
        }
    }

    void Update()
    {
        if (isPressed)
        {
            if (onButtonHeld != null) onButtonHeld.Invoke();

            if (Time.time - lastTouchTime > releaseTolerance)
            {
                ReleaseButton();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerFinger"))
        {
            lastTouchTime = Time.time;

            if (!isPressed)
            {
                PressButton();
            }
        }
    }

    private void PressButton()
    {
        isPressed = true;
        buttonCap.localPosition = originalPosition - new Vector3(0, 0, pressDownDistance);
        if (onButtonDown != null) onButtonDown.Invoke();
    }

    private void ReleaseButton()
    {
        isPressed = false;
        buttonCap.localPosition = originalPosition;
        if (onButtonUp != null) onButtonUp.Invoke();
    }
}
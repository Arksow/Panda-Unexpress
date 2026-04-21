using UnityEngine;

public class TaskHighlight : MonoBehaviour
{
    public Outline outlineComponent;

    public bool isPulsing = true;
    public float minWidth = 5f;
    public float maxWidth = 15f;
    public float pulseSpeed = 5f;

    private void Awake()
    {
        SetHighlight(false);
    }

    private void Update()
    {
        if (isPulsing && outlineComponent != null && outlineComponent.enabled)
        {
            float lerp = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            outlineComponent.OutlineWidth = Mathf.Lerp(minWidth, maxWidth, lerp);
        }
    }

    public void SetHighlight(bool isActive)
    {
        if (outlineComponent != null)
        {
            if (isActive/* && !RunSettings.OutlinesEnabled*/)
            {
                outlineComponent.enabled = false;
                return;
            }

            outlineComponent.enabled = isActive;
        }
    }
}
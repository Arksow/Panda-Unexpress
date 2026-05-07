using UnityEngine;
using Oculus.Interaction;

public class SpigotDispenser : MonoBehaviour
{
    public LiquidBase barrelLiquidType;
    public MetaSocket cupSocket;

    public ParticleSystem liquidStream;
    public float fillSpeed = 0.2f;

    public Transform spigotHandle;

    public float pourAngleThreshold = 45f;
    public float springSpeed = 10f;

    private Grabbable handleGrabbable;
    public bool isPouring = false;

    void Awake()
    {
        if (liquidStream != null) liquidStream.Stop();
        if (spigotHandle != null)
        {
            handleGrabbable = spigotHandle.GetComponent<Grabbable>();
        }
    }

    protected virtual void Update()
    {
        if (spigotHandle != null)
        {
            bool isBeingGrabbed = handleGrabbable != null && handleGrabbable.SelectingPointsCount > 0;

            if (!isBeingGrabbed)
            {
                float currentX = spigotHandle.localEulerAngles.x;
                float newX = Mathf.LerpAngle(currentX, 0f, Time.deltaTime * springSpeed);
                spigotHandle.localEulerAngles = new Vector3(newX, spigotHandle.localEulerAngles.y, spigotHandle.localEulerAngles.z);
            }

            float currentAngle = spigotHandle.localEulerAngles.x;

            if (currentAngle > 180f) currentAngle -= 360f;

            isPouring = currentAngle > pourAngleThreshold;
        }

        if (isPouring && !liquidStream.isPlaying) liquidStream.Play();
        else if (!isPouring && liquidStream.isPlaying) liquidStream.Stop();

        if (isPouring && cupSocket.HasItem())
        {
            CupData cup = cupSocket.GetSocketItem();

            if (cup != null)
            {
                cup.AddLiquid(barrelLiquidType, fillSpeed * Time.deltaTime);
            }
        }
    }
}
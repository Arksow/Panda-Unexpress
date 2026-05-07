using UnityEngine;
using Oculus.Interaction;

public class SpigotDispenser : MonoBehaviour
{
    [Header("Dispenser Settings")]
    public LiquidBase barrelLiquidType;
    public MetaSocket cupSocket;

    public ParticleSystem liquidStream;
    public float fillSpeed = 0.2f;
    public Transform spigotHandle;
    public float pourAngleThreshold = 45f;

    public bool isPouring = false;

    void Awake()
    {
        if (liquidStream != null) liquidStream.Stop();
    }

    protected virtual void Update()
    {
        if (spigotHandle != null)
        {
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
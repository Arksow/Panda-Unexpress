using UnityEngine;
using Oculus.Interaction;

public class SpigotDispenser : MonoBehaviour
{
    public LiquidBase barrelLiquidType;
    public MetaSocket cupSocket;

    public ParticleSystem liquidStream;
    public float fillSpeed = 15f;

    public Transform spigotHandle;
    public float pourAngleThreshold = 45f;
    public float springSpeed = 10f;

    [Header("Audio")]
    public AudioSource spigotAudioSource;
    public AudioClip dispensingSound;

    private Grabbable handleGrabbable;
    public bool isPouring = false;

    private Quaternion restingRotation;
    private Vector3 restingPosition;

    void Awake()
    {
        if (liquidStream != null) liquidStream.Stop();
        if (spigotHandle != null)
        {
            handleGrabbable = spigotHandle.GetComponent<Grabbable>();
            restingRotation = spigotHandle.localRotation;
            restingPosition = spigotHandle.localPosition;
        }
    }

    protected virtual void Update()
    {
        if (spigotHandle != null)
        {
            bool isBeingGrabbed = handleGrabbable != null && handleGrabbable.SelectingPointsCount > 0;

            if (!isBeingGrabbed)
            {
                spigotHandle.localRotation = Quaternion.Lerp(spigotHandle.localRotation, restingRotation, Time.deltaTime * springSpeed);
                spigotHandle.localPosition = Vector3.Lerp(spigotHandle.localPosition, restingPosition, Time.deltaTime * springSpeed);
            }

            float pullDistance = Quaternion.Angle(restingRotation, spigotHandle.localRotation);
            isPouring = pullDistance > pourAngleThreshold;
        }

        if (liquidStream != null)
        {
            if (isPouring && !liquidStream.isPlaying)
            {
                liquidStream.Play();

                if (spigotAudioSource != null && dispensingSound != null)
                {
                    spigotAudioSource.clip = dispensingSound;
                    spigotAudioSource.loop = true;
                    if (!spigotAudioSource.isPlaying)
                    {
                        spigotAudioSource.Play();
                    }
                }
            }
            else if (!isPouring && liquidStream.isPlaying)
            {
                liquidStream.Stop();

                if (spigotAudioSource != null)
                {
                    spigotAudioSource.Stop();
                }

                if (cupSocket != null && cupSocket.HasItem())
                {
                    CupData cup = cupSocket.GetSocketItem();
                    if (cup != null)
                    {
                        if (cup.base1 == barrelLiquidType) cup.base1Amount = 0.5f;
                        else if (cup.base2 == barrelLiquidType) cup.base2Amount = 0.5f;

                        cup.UpdateUI();
                    }
                }
            }
        }

        if (isPouring && cupSocket != null && cupSocket.HasItem())
        {
            CupData cup = cupSocket.GetSocketItem();
            if (cup != null && !cup.isSealed)
            {
                cup.AddLiquid(barrelLiquidType, fillSpeed * Time.deltaTime);
            }
        }
    }
}
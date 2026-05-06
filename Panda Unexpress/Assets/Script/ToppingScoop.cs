using UnityEngine;

public class ToppingScoop : MonoBehaviour
{
    public ParticleSystem scoopEmitter;

    public int heldParticles = 0;
    public float tiltThreshold = 0.5f;

    void Update()
    {
        if (heldParticles > 0)
        {
            float tilt = Vector3.Dot(transform.up, Vector3.down);
            if (tilt > tiltThreshold)
            {
                scoopEmitter.Emit(heldParticles);
                heldParticles = 0;
            }
        }

        if (EventManager.instance != null && EventManager.instance.currentEvent == Events.LeakyScoop)
        {
            if (heldParticles > 0 && Time.frameCount % 30 == 0)
            {
                heldParticles--;
                scoopEmitter.Emit(1);
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

public class BobaBin : MonoBehaviour
{
    public ToppingScoop targetScoop;

    private ParticleSystem binParticles;
    private List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();

    void Start()
    {
        binParticles = GetComponent<ParticleSystem>();
    }

    protected virtual void OnParticleTrigger()
    {
        int numEnter = binParticles.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);

        if (numEnter > 0 && targetScoop != null)
        {
            targetScoop.heldParticles += numEnter;
            Debug.Log($"Caught {numEnter} boba! Total held: {targetScoop.heldParticles}");

            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle p = enterParticles[i];
                p.remainingLifetime = 0f;
                enterParticles[i] = p;
            }

            binParticles.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);
        }
    }
}
using UnityEngine;

public class BobaPourer : MonoBehaviour
{
    public virtual void OnParticleCollision(GameObject other)
    {
        CupData cup = other.GetComponentInParent<CupData>();

        if (cup != null)
        {
            cup.AddBobaParticles(1);
        }
    }
}
using UnityEngine;

public class BobaPourer : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        CupData cup = other.GetComponentInParent<CupData>();

        if (cup != null)
        {
            cup.AddBobaParticles(1);
        }
    }
}
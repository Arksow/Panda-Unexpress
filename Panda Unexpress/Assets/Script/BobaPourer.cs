using UnityEngine;

public class BobaPourer : MonoBehaviour
{
    public  void OnParticleCollision(GameObject other)
    {
       
        CupData cup = other.GetComponentInParent<CupData>();

        if (cup != null)
        {
            cup.AddBobaParticles(1);
        }
    }
}
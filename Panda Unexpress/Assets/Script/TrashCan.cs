using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public AudioClip trashSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cup"))
        {
            CupData cup = other.GetComponent<CupData>();

            if (cup != null)
            {
                if (SoundManager.instance != null && trashSound != null)
                {
                    SoundManager.instance.PlaySpatialSFX(trashSound, transform.position);
                }

                Destroy(other.gameObject);
            }
        }
    }
}
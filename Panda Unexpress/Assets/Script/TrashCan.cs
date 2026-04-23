using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public AudioClip trashSound;
    private int cupLayer;

    private void Start()
    {
        cupLayer = LayerMask.NameToLayer("Cup");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == cupLayer)
        {
            CupData cup = other.GetComponent<CupData>();

            if (cup != null)
            {
                if (AudioController.Instance != null && trashSound != null)
                {
                    AudioController.Instance.PlaySpatialSFX(trashSound, transform.position);
                }

                Destroy(other.gameObject);
            }
        }
    }
}
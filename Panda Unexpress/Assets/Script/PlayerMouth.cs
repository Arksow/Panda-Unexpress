using UnityEngine;

public class PlayerMouth : MonoBehaviour
{
    public AudioClip drinkingSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cup"))
        {
            CupData cup = other.GetComponent<CupData>();

            if (cup != null && (cup.base1Amount > 0 || cup.bobaParticleCount > 0))
            {
                if (EventManager.instance != null && EventManager.instance.currentEvent == Events.ThirstyPlayer)
                {
                    Debug.Log("🧋 Slurp! The player quenched their thirst.");

                    if (AudioController.Instance != null && drinkingSound != null)
                    {
                        AudioController.Instance.PlayGlobalSFX(drinkingSound);
                    }

                    EventManager.instance.ResolveCurrentEvent();
                    Destroy(other.gameObject);
                }
                else
                {
                    Debug.Log("I'm not thirsty right now, I have to work!");
                }
            }
        }
    }
}
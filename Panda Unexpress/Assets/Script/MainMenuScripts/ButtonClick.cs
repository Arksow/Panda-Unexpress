using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public AudioClip clickSound;

    public void PlaySfx()
    {
        AudioController.Instance.PlayGlobalSFX(clickSound);
    }
}

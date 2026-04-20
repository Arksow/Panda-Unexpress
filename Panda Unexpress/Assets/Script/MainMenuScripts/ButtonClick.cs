using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public string sfxName;

    public void PlaySfx()
    {
        AudioController.Instance.PlaySFX(sfxName);
    }
}

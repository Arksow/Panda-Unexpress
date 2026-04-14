using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string name;
    public void Start()
    {
        AudioController.Instance.PlayMusic(name);
    }
}

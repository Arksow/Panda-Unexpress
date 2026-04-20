using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
   public void LoadSceneByName(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}

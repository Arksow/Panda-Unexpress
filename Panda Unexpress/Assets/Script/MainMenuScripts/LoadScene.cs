using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void TriggerStartShift()
    {
        if (SceneTransitionManager.instance != null)
            SceneTransitionManager.instance.StartShift();
    }

    public void TriggerStartTutorial()
    {
        if (SceneTransitionManager.instance != null)
            SceneTransitionManager.instance.StartTutorial();
    }
}

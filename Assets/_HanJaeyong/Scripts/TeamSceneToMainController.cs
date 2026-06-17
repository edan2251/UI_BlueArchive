using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamSceneToMainController : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Main";

    public void OnClickGoBackToMain()
    {
        if (GameTransitionManager.Instance == null || GameTransitionManager.Instance.IsTransitioning) return;

        GameTransitionManager.Instance.TransitionTo(TransitionType.SceneSwap, () =>
        {
            SceneManager.LoadScene(mainSceneName);
            GameTransitionManager.Instance.HideTransition();
        });
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneController : MonoBehaviour
{
    public void OnClickGoToStudentList()
    {
        if (GameTransitionManager.Instance == null || GameTransitionManager.Instance.IsTransitioning) return;

        GameTransitionManager.Instance.TransitionTo(TransitionType.SceneSwap, () =>
        {
            SceneManager.LoadScene("StudentList");

            GameTransitionManager.Instance.HideTransition();
        });
    }

    public void OnClickGoToWooPyunHaam()
    {
        if (GameTransitionManager.Instance == null || GameTransitionManager.Instance.IsTransitioning) return;

        GameTransitionManager.Instance.TransitionTo(TransitionType.SceneSwap, () =>
        {
            SceneManager.LoadScene("WooPyunHaam");
            GameTransitionManager.Instance.HideTransition();
        });
    }

    public void OnClickGoToShop()
    {
        if (GameTransitionManager.Instance == null || GameTransitionManager.Instance.IsTransitioning) return;

        GameTransitionManager.Instance.TransitionTo(TransitionType.SceneSwap, () =>
        {
            SceneManager.LoadScene("Shop");
            GameTransitionManager.Instance.HideTransition();
        });
    }

    public void OnClickGoToARONA()
    {
        if (GameTransitionManager.Instance == null || GameTransitionManager.Instance.IsTransitioning) return;

        GameTransitionManager.Instance.TransitionTo(TransitionType.SceneSwap, () =>
        {
            SceneManager.LoadScene("ARONA");
            GameTransitionManager.Instance.HideTransition();
        });
    }
}
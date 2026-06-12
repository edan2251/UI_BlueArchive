using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState
{ 
    StudentList,
    StudentDetail
}

public class StudentSceneController : MonoBehaviour
{
    public static StudentSceneController Instance;

    [HideInInspector] public UIState currentState = UIState.StudentList;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void OnClickGlobalBackButton()
    {
        if (GameTransitionManager.Instance == null || GameTransitionManager.Instance.IsTransitioning) return;

        if (currentState == UIState.StudentList)
        {
            GameTransitionManager.Instance.TransitionTo(TransitionType.SceneSwap, () =>
            {
                SceneManager.LoadScene("Main");

                GameTransitionManager.Instance.HideTransition();
            });
        }
        else if (currentState == UIState.StudentDetail)
        {
            GameTransitionManager.Instance.TransitionTo(TransitionType.CanvasSwap, () =>
            {
                if (StudentDetailUI.Instance != null)
                {
                    StudentDetailUI.Instance.CloseDetail();
                }

                GameTransitionManager.Instance.HideTransition();
            });
        }
    }
}
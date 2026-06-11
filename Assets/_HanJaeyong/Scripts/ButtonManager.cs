using TransitionScreenPackage;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum UIState
{
    StudentList,
    StudentDetail
}

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;

    [HideInInspector] public UIState currentState = UIState.StudentList;

    [Header("트랜지션 연출에셋")]
    public TransitionScreenManager transitionManager;

    public bool isTransitioning { get; private set; } = false;
    private Action onRevealAction;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (transitionManager != null)
        {
            transitionManager.FinishedRevealEvent += OnScreenCovered;
            transitionManager.FinishedHideEvent += OnTransitionFinished;
        }

    }

    public void TransitionTo(Action actionToExecute)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        onRevealAction = actionToExecute;

        if (transitionManager != null)
        {
            //화면 덮기
            transitionManager.Reveal();
        }
    }

    //뒤로가기 버튼 눌렀을 때
    public void OnClickGlobalBackButton()
    {
        TransitionTo(() =>
        {
            switch (currentState)
            {
                case UIState.StudentList:
                    SceneManager.LoadScene("Main");
                    break;

                case UIState.StudentDetail:
                    if (StudentDetailUI.Instance != null)
                    {
                        StudentDetailUI.Instance.CloseDetail();
                    }
                    if (transitionManager != null)
                    {
                        transitionManager.Hide();
                    }
                    break;
            }
        });
    }

    private void ExecuteBackAction()
    {
        switch (currentState)
        {
            case UIState.StudentList:
                SceneManager.LoadScene("Main");
                break;

            case UIState.StudentDetail:
                if (StudentDetailUI.Instance != null)
                {
                    StudentDetailUI.Instance.CloseDetail();
                }

                if (transitionManager != null)
                {
                    //화면 걷기
                    transitionManager.Hide();
                }
                break;
        }
    }

    private void OnScreenCovered()
    {
        if (onRevealAction != null)
        {
            onRevealAction.Invoke();
            onRevealAction = null;
        }
    }

    private void OnTransitionFinished()
    {
        isTransitioning = false;
    }

    private void OnDestroy()
    {
        if (transitionManager != null)
        {
            transitionManager.FinishedRevealEvent -= ExecuteBackAction;
            transitionManager.FinishedHideEvent -= OnTransitionFinished;
        }
    }
}
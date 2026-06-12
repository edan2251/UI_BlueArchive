using System;
using UnityEngine;
using TransitionScreenPackage;

public enum TransitionType
{
    SceneSwap,
    CanvasSwap
}

public class GameTransitionManager : MonoBehaviour
{
    public static GameTransitionManager Instance;

    [Header("연출 에셋 연결")]
    public TransitionScreenManager sceneTransition;
    public TransitionScreenManager canvasTransition;

    public bool IsTransitioning { get; private set; } = false;
    private Action onRevealAction;
    private TransitionScreenManager activeTransition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            RegisterEvents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RegisterEvents()
    {
        if (sceneTransition != null)
        {
            sceneTransition.FinishedRevealEvent += OnScreenCovered;
            sceneTransition.FinishedHideEvent += OnTransitionFinished;
        }
        if (canvasTransition != null)
        {
            canvasTransition.FinishedRevealEvent += OnScreenCovered;
            canvasTransition.FinishedHideEvent += OnTransitionFinished;
        }
    }

    public void TransitionTo(TransitionType type, Action actionToExecute)
    {
        if (IsTransitioning) return;

        IsTransitioning = true;
        onRevealAction = actionToExecute;

        activeTransition = (type == TransitionType.SceneSwap) ? sceneTransition : canvasTransition;

        if (activeTransition != null)
        {
            activeTransition.Reveal();
        }
    }

    public void HideTransition()
    {
        if (activeTransition != null)
        {
            activeTransition.Hide();
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
        IsTransitioning = false;
        activeTransition = null;
    }

    private void OnDestroy()
    {
        if (sceneTransition != null)
        {
            sceneTransition.FinishedRevealEvent -= OnScreenCovered;
            sceneTransition.FinishedHideEvent -= OnTransitionFinished;
        }
        if (canvasTransition != null)
        {
            canvasTransition.FinishedRevealEvent -= OnScreenCovered;
            canvasTransition.FinishedHideEvent -= OnTransitionFinished;
        }
    }
}
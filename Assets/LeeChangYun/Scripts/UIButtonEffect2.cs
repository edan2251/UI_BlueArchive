using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIButtonEffect2 : MonoBehaviour
{
    private Button myButton;
    private RectTransform buttonRect;
    private Vector3 originalScale;

    [SerializeField] private float pressedScale = 0.9f;
    [SerializeField] private float duration = 0.1f;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        buttonRect = GetComponent<RectTransform>();
        originalScale = buttonRect.localScale;

        if (myButton != null)
        {
            myButton.onClick.AddListener(PlayClickEffect);
        }
    }

    public void PlayClickEffect()
    {
        buttonRect.DOKill();
        buttonRect.localScale = originalScale;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(buttonRect.DOScale(originalScale * pressedScale, duration));
        sequence.Append(buttonRect.DOScale(originalScale, duration));
    }

    private void OnDestroy()
    {
        if (myButton != null)
        {
            myButton.onClick.RemoveListener(PlayClickEffect);
        }
    }
}
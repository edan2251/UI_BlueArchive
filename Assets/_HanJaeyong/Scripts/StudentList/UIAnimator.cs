using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    [Header("[리스트 등장 수치]")]
    [Tooltip("애니메이션 지속 시간"), SerializeField] private float listDuration = 0.35f;
    [Tooltip("슬롯 간 등장 간격"), SerializeField] private float listStaggerDelay = 0.04f;
    [Tooltip("효과 함수"), SerializeField] private Ease listEase = Ease.OutBack;
    [Range(0f, 1f)]
    [Tooltip("시작 투명도"), SerializeField] private float listInitialAlpha = 0f;

    [Header("[자리 이동 연출]")]
    [Tooltip("스르륵 이동하는 시간"), SerializeField] private float reorderDuration = 0.3f;
    [Tooltip("부드러운 감속 이동"), SerializeField] private Ease reorderEase = Ease.OutCubic;

    [Header("[핀 클릭 Down 수치]")]
    [Tooltip("눌렀을 때 작아지는 크기"), SerializeField] private float pinDownScale = 0.8f;
    [Tooltip("눌리는 속도"), SerializeField] private float pinDownDuration = 0.1f;

    [Header("[핀 클릭 Up 수치]")]
    [Tooltip("복구 시 탄성 시간"), SerializeField] private float pinUpBounceDuration = 0.5f;
    [Tooltip("탄성 효고"), SerializeField] private Ease pinUpBounceEase = Ease.OutElastic;

    [Tooltip("즐겨찾기 켰을 때 색상"), SerializeField] private Color pinActiveColor = Color.white;
    [Tooltip("껐을 때 색상"), SerializeField] private Color pinInactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [Tooltip("색상 변경 속도"), SerializeField] private float pinColorDuration = 0.2f;

    [Header("[슬라이드 전환 연출]")]
    [Tooltip("사라지고 나타나는 시간"), SerializeField] private float slideDuration = 0.15f;
    [Tooltip("화면 밖으로 나갈 거리"), SerializeField] private float slideOffset = 1000f;

    [Header("[레벨업 연출 수치]")]
    [Tooltip("슬라이더가 차오르는 속도"), SerializeField] private float expDuration = 0.3f;
    [Tooltip("슬라이더 연출 이징"), SerializeField] private Ease expEase = Ease.OutQuad;
    [Tooltip("레벨업 텍스트 펀치크기"), SerializeField] private float lvlPunchScale = 0.3f;
    [Tooltip("레벨업 텍스트 펀치시간"), SerializeField] private float lvlPunchDuration = 0.3f;

    //리스트 순차 등장
    public void PlayListPopIn(List<GameObject> activeSlots)
    {

        for (int i = 0; i < activeSlots.Count; i++)
        {
            Transform slotTransform = activeSlots[i].transform;

            CanvasGroup cg = activeSlots[i].GetComponent<CanvasGroup>();
            if (cg == null) cg = activeSlots[i].AddComponent<CanvasGroup>();

            slotTransform.DOKill();
            cg.DOKill();

            slotTransform.localScale = Vector3.zero;
            cg.alpha = listInitialAlpha;

            float delayTime = i * listStaggerDelay;

            slotTransform.DOScale(Vector3.one, listDuration)
                         .SetEase(listEase)
                         .SetDelay(delayTime);

            cg.DOFade(1f, listDuration)
              .SetDelay(delayTime);
        }
    }

    //자리 이동 연출
    public void PlayLayoutReorder(Dictionary<Transform, Vector3> oldLocalPositions, RectTransform contentRect)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        foreach (Transform child in contentRect)
        {
            if (!child.gameObject.activeInHierarchy) continue;

            if (oldLocalPositions.TryGetValue(child, out Vector3 oldLocalPos))
            {
                Vector3 targetLocalPos = child.localPosition;

                child.localPosition = oldLocalPos;

                child.DOKill();
                child.DOLocalMove(targetLocalPos, reorderDuration).SetEase(reorderEase);
            }
        }
    }

    //핀 클릭 Down
    public void PlayPinDown(Transform pinTransform)
    {
        pinTransform.DOKill();
        pinTransform.DOScale(pinDownScale, pinDownDuration).SetEase(Ease.OutQuad);
    }

    //핀 클릭 Up
    public void PlayPinUp(Transform pinTransform, Image pinImage, bool isActive)
    {
        pinTransform.DOKill();
        pinImage.DOKill();

        pinTransform.DOScale(Vector3.one, pinUpBounceDuration).SetEase(pinUpBounceEase);

        Color targetColor = isActive ? pinActiveColor : pinInactiveColor;
        pinImage.DOColor(targetColor, pinColorDuration);
    }

    //핀 기본 설정
    public void SetPinColorInstant(Image pinImage, bool isActive)
    {
        if (pinImage == null) return;

        pinImage.DOKill();
        pinImage.color = isActive ? pinActiveColor : pinInactiveColor;
    }
    public void PlayTextPinUp(Transform pinTransform, TextMeshProUGUI textUI, bool isActive, Color activeColor, Color inactiveColor)
    {
        pinTransform.DOKill();
        if (textUI != null) textUI.DOKill();

        pinTransform.DOScale(Vector3.one, pinUpBounceDuration).SetEase(pinUpBounceEase);

        if (textUI != null)
        {
            Color targetColor = isActive ? activeColor : inactiveColor;
            textUI.DOColor(targetColor, pinColorDuration);
        }
    }

    public void SetTextPinColorInstant(TextMeshProUGUI textUI, bool isActive, Color activeColor, Color inactiveColor)
    {
        if (textUI == null) return;
        textUI.DOKill();
        textUI.color = isActive ? activeColor : inactiveColor;
    }

    public void PlaySlideTransition(RectTransform panelRect, bool isNext, TweenCallback onSwapData)
    {
        CanvasGroup cg = panelRect.GetComponent<CanvasGroup>();
        if (cg == null) cg = panelRect.AddComponent<CanvasGroup>();

        panelRect.DOKill();
        cg.DOKill();

        float directionMultiplier = isNext ? -1f : 1f;
        float startX = 0f;

        DG.Tweening.Sequence seq = DOTween.Sequence();

        seq.Append(panelRect.DOAnchorPosX(startX + (slideOffset * directionMultiplier), slideDuration).SetEase(Ease.InBack));
        seq.Join(cg.DOFade(0f, slideDuration));

        seq.AppendCallback(onSwapData);

        seq.AppendCallback(() =>
        {
            panelRect.anchoredPosition = new Vector2(startX + (slideOffset * -directionMultiplier), panelRect.anchoredPosition.y);
            cg.alpha = 0f;
        });

        seq.Append(panelRect.DOAnchorPosX(startX, slideDuration).SetEase(Ease.OutBack));
        seq.Join(cg.DOFade(1f, slideDuration));
    }

    public bool PlayExpGainAnimation(Slider slider, TextMeshProUGUI lvlText, float startExp, float addExp, int nextLevel, System.Action onLevelUpDataUpdate)
    {
        slider.DOKill();
        if (lvlText != null) lvlText.transform.DOKill();

        float targetExp = startExp + addExp;
        float maxExp = slider.maxValue;

        DG.Tweening.Sequence seq = DOTween.Sequence();

        if (targetExp >= maxExp)
        {
            float remainderExp = targetExp - maxExp;

            seq.Append(slider.DOValue(maxExp, expDuration).SetEase(expEase));

            seq.AppendCallback(() =>
            {
                onLevelUpDataUpdate?.Invoke();

                if (lvlText != null)
                {
                    lvlText.text = $"LV.{nextLevel}";
                    lvlText.transform.localScale = Vector3.one;
                    lvlText.transform.DOPunchScale(new Vector3(lvlPunchScale, lvlPunchScale, 0), lvlPunchDuration, 5);
                }
                slider.value = 0f;
            });

            seq.Append(slider.DOValue(remainderExp, expDuration).SetEase(expEase));
        }
        else
        {
            seq.Append(slider.DOValue(targetExp, expDuration).SetEase(expEase));
            seq.AppendCallback(() =>
            {
                onLevelUpDataUpdate?.Invoke();
            });
        }

        return true;
    }

    public void PlayStatCounterAnimation(TextMeshProUGUI textUI, float startValue, float endValue, bool isInt)
    {
        textUI.DOKill();

        DOTween.To(() => startValue, x =>
        {
            if (isInt) textUI.text = Mathf.RoundToInt(x).ToString();
            else textUI.text = x.ToString("0.##") + "%";
        }, endValue, 0.5f).SetEase(Ease.OutCubic);
    }
}
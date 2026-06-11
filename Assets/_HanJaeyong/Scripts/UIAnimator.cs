using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class UIAnimator : MonoBehaviour
{
    [Header("[리스트 등장 수치]")]
    [Tooltip("애니메이션 지속 시간"), SerializeField] private         float listDuration = 0.35f;
    [Tooltip("슬롯 간 등장 간격"), SerializeField] private           float listStaggerDelay = 0.04f;
    [Tooltip("효과 함수"), SerializeField] private                   Ease listEase = Ease.OutBack;
    [Range(0f, 1f)]
    [Tooltip("시작 투명도"), SerializeField] private                 float listInitialAlpha = 0f;

    [Header("[핀 클릭 Down 수치]")]
    [Tooltip("눌렀을 때 작아지는 크기"), SerializeField] private      float pinDownScale = 0.8f;
    [Tooltip("눌리는 속도"), SerializeField] private                 float pinDownDuration = 0.1f;

    [Header("[핀 클릭 Up 수치]")]
    [Tooltip("복구 시 탄성 시간"), SerializeField] private           float pinUpBounceDuration = 0.5f;
    [Tooltip("탄성 효고"), SerializeField] private                   Ease pinUpBounceEase = Ease.OutElastic;

    [Tooltip("즐겨찾기 켰을 때 색상"), SerializeField] private        Color pinActiveColor = Color.white;
    [Tooltip("껐을 때 색상"), SerializeField] private                Color pinInactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    [Tooltip("색상 변경 속도"), SerializeField] private              float pinColorDuration = 0.2f;


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
}
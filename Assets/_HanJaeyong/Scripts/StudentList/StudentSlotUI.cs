using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("UI 연결")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Image favoritePinImage;

    public GameObject pinClickAreaObject;

    private Student myStudent;
    private StudentListManager manager;
    private UIAnimator animator;

    public void Setup(Student student, StudentListManager listManager, UIAnimator uiAnimator)
    {
        myStudent = student;
        manager = listManager;
        animator = uiAnimator;
        UpdateUI();
    }

    public void UpdateUI()
    {
        nameText.text = myStudent.OriginData.studentName;
        levelText.text = $"Lv.{myStudent.CurrentLevel}";

        if (animator != null && favoritePinImage != null)
        {
            animator.SetPinColorInstant(favoritePinImage, myStudent.IsFavorite);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //클릭한곳이 pin영역이면 리턴
        if (eventData.pointerPressRaycast.gameObject == pinClickAreaObject) return;

        //버튼 없애고 버튼 효과 구현
        transform.DOPunchScale(new Vector3(-0.05f, -0.05f, 0), 0.2f);

        //0.15초 이따가 넘어가기
        Invoke("OnClickDetail", 0.15f); OnClickDetail();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerPressRaycast.gameObject != pinClickAreaObject) return;

        //Down애니메이션
        if (animator != null && favoritePinImage != null)
        {
            animator.PlayPinDown(favoritePinImage.transform);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerPressRaycast.gameObject != pinClickAreaObject) return;

        myStudent.ToggleFavorite();

        //Up애니메이션
        if (animator != null && favoritePinImage != null)
        {
            animator.PlayPinUp(favoritePinImage.transform, favoritePinImage, myStudent.IsFavorite);
        }

        if (manager != null)
        {
            manager.SortAndRefreshUI(false, myStudent.IsFavorite);
        }
    }

    public void OnClickDetail()
    {
        if (GameTransitionManager.Instance != null)
        {
            GameTransitionManager.Instance.TransitionTo(TransitionType.CanvasSwap, () =>
            {
                if (StudentDetailUI.Instance != null)
                {
                    StudentDetailUI.Instance.OpenDetail(myStudent);
                }

                GameTransitionManager.Instance.HideTransition();
            });
        }
    }
}
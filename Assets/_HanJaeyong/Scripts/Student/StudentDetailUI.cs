using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StudentDetailUI : MonoBehaviour
{
    public static StudentDetailUI Instance;

    [Header("[캔버스 전환]")]
    [SerializeField] private GameObject studentListCanvas;
    [SerializeField] private GameObject studentDetailCanvas;

    [Header("[셀렉트 버튼 UI]")]
    [SerializeField] private Image selectButtonImage;
    [SerializeField] private TextMeshProUGUI selectButtonText;
    [SerializeField] private Color textInactiveColor = Color.gray;

    [Header("[애니메이터 연결]")]
    [SerializeField] private UIAnimator uiAnimator;

    [Header("[슬라이드 연출]")]
    [SerializeField] private RectTransform detailPanelRect;

    [Header("[기본 정보]")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI mysteryLevelText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI roleText;
    public Slider expSlider;

    [Header("[기본 능력치]")]
    public TextMeshProUGUI maxHpText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI healingText;

    [Header("[무기 정보]")]
    public Image weaponImage;
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponAttackText;
    public TextMeshProUGUI weaponCritRateText;
    public TextMeshProUGUI weaponPenetrationText;
    public TextMeshProUGUI weaponRangeText;

    private Student currentStudent;
    private Color originalTextColor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (selectButtonText != null)
        {
            originalTextColor = selectButtonText.color;
        }
    }

    public void OpenDetail(Student student)
    {
        studentListCanvas.SetActive(false);
        studentDetailCanvas.SetActive(true);

        if (detailPanelRect != null)
        {
            detailPanelRect.anchoredPosition = Vector2.zero;

            CanvasGroup cg = detailPanelRect.GetComponent<CanvasGroup>();
            if (cg != null) cg.alpha = 1f;
        }

        BindData(student);

        if (ButtonManager.Instance != null)
        {
            ButtonManager.Instance.currentState = UIState.StudentDetail;
        }
    }

    private void BindData(Student student)
    {
        currentStudent = student;

        //데이터 적용
        nameText.text = student.OriginData.studentName;
        mysteryLevelText.text = student.OriginData.mysteryLevel.ToString();
        levelText.text = $"LV.{student.CurrentLevel}";
        roleText.text = student.OriginData.role.ToString();

        // 경험치
        expSlider.maxValue = 1000f; //최대 경험치
        expSlider.value = student.OriginData.exp;

        // 능력치
        maxHpText.text = student.OriginData.baseStats.maxHP.ToString();
        attackText.text = student.OriginData.baseStats.attack.ToString();
        defenseText.text = student.OriginData.baseStats.defense.ToString();
        healingText.text = student.OriginData.baseStats.healing.ToString();

        // 무기
        weaponNameText.text = student.OriginData.weaponStats.weaponName;
        weaponAttackText.text = student.OriginData.weaponStats.attack.ToString();
        weaponCritRateText.text = $"{student.OriginData.weaponStats.critRate}%";
        weaponPenetrationText.text = student.OriginData.weaponStats.armorPenetration.ToString();
        weaponRangeText.text = student.OriginData.weaponStats.range.ToString();

        if (student.OriginData.weaponStats.weaponImage != null)
        {
            weaponImage.sprite = student.OriginData.weaponStats.weaponImage;
            weaponImage.enabled = true;
        }
        else
        {
            weaponImage.enabled = false;
        }

        UpdateSelectButtonVisualInstant();
    }

    private void UpdateSelectButtonVisualInstant()
    {
        if (currentStudent == null || selectButtonImage == null || uiAnimator == null) return;

        uiAnimator.SetTextPinColorInstant(selectButtonText, currentStudent.IsFavorite, originalTextColor, textInactiveColor);
    }

    //셀렉트버튼 눌렀을때 Down
    public void PlaySelectDownAnim()
    {
        if (uiAnimator != null && selectButtonImage != null)
        {
            uiAnimator.PlayPinDown(selectButtonImage.transform);
        }
    }

    //셀렉트버튼에서 손뗐을때 Up
    public void PlaySelectUpAnim()
    {
        if (currentStudent == null || selectButtonImage == null) return;

        currentStudent.ToggleFavorite();

        if (uiAnimator != null)
        {
            uiAnimator.PlayTextPinUp(selectButtonImage.transform, selectButtonText, currentStudent.IsFavorite, originalTextColor, textInactiveColor);
        }

        if (StudentListManager.Instance != null)
        {
            StudentListManager.Instance.SortAndRefreshUI(false, false);
        }
    }

    //오른쪽 버튼 눌렀을 때
    public void OnClickNextStudent()
    {
        if (currentStudent == null || StudentListManager.Instance == null) return;

        List<Student> activeList = StudentListManager.Instance.CurrentActiveList;
        if (activeList == null || activeList.Count <= 1) return;

        int currentIndex = activeList.IndexOf(currentStudent);
        if (currentIndex == -1) return;

        int nextIndex = (currentIndex + 1) % activeList.Count;

        if (uiAnimator != null && detailPanelRect != null)
        {
            uiAnimator.PlaySlideTransition(detailPanelRect, true, () => BindData(activeList[nextIndex]));
        }
    }

    // 왼쪽 버튼 눌렀을 때
    public void OnClickPrevStudent()
    {
        if (currentStudent == null || StudentListManager.Instance == null) return;

        List<Student> activeList = StudentListManager.Instance.CurrentActiveList;
        if (activeList == null || activeList.Count <= 1) return;

        int currentIndex = activeList.IndexOf(currentStudent);
        if (currentIndex == -1) return;

        int prevIndex = (currentIndex - 1 + activeList.Count) % activeList.Count;

        if (uiAnimator != null && detailPanelRect != null)
        {
            uiAnimator.PlaySlideTransition(detailPanelRect, false, () => BindData(activeList[prevIndex]));
        }
    }

    public void CloseDetail()
    {
        studentDetailCanvas.SetActive(false);
        studentListCanvas.SetActive(true);

        if (StudentListManager.Instance != null)
        {
            StudentListManager.Instance.SortAndRefreshUI(false, true);
        }

        currentStudent = null;

        if (ButtonManager.Instance != null)
        {
            ButtonManager.Instance.currentState = UIState.StudentList;
        }
    }
}
using DG.Tweening;
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

    [Header("[메인 탭 버튼]")]
    [SerializeField] private Sprite mainTabActiveSprite;
    [SerializeField] private Sprite mainTabInactiveSprite;

    [Header("[메인 탭 UI (0:기본, 1:레벨업, 2:신비)]")]
    [SerializeField] private Image[] mainTabButtons;
    [SerializeField] private GameObject[] mainTabPanels;

    [Header("[서브 탭 버튼]")]
    [SerializeField] private Sprite subTabActiveSprite;
    [SerializeField] private Sprite subTabInactiveSprite;

    [Header("[서브 탭 UI (0:스킬, 1:무기, 2:방어구)]")]
    [SerializeField] private Image[] subTabButtons;
    [SerializeField] private GameObject[] subTabPanels;

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

    [Header("[무기 강화 정보]")]
    [SerializeField] private TextMeshProUGUI nextWeaponAttackText;
    [SerializeField] private TextMeshProUGUI nextWeaponCritRateText;
    [SerializeField] private TextMeshProUGUI nextWeaponPenetrationText;
    [SerializeField] private TextMeshProUGUI nextWeaponRangeText;


    private Student currentStudent;
    private Color originalTextColor;
    private bool isExpAnimating = false;

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

        isExpAnimating = false;
        BindData(student);

        if (StudentSceneController.Instance != null)
        {
            StudentSceneController.Instance.currentState = UIState.StudentDetail;
        }

        OnClickMainTab(0);
        OnClickSubTab(1);
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
        weaponCritRateText.text = $"{student.OriginData.weaponStats.critRate:0}%";
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
        UpdateWeaponUpgradePreviews();
    }

    private void UpdateSelectButtonVisualInstant()
    {
        if (currentStudent == null || selectButtonImage == null || uiAnimator == null) return;

        uiAnimator.SetTextPinColorInstant(selectButtonText, currentStudent.IsFavorite, originalTextColor, textInactiveColor);
    }

    public void OnClickWeaponUpgradeButton()
    {
        if (currentStudent == null || uiAnimator == null) return;

        var weapon = currentStudent.OriginData.weaponStats;

        float oldAttack = weapon.attack;
        float oldCrit = weapon.critRate;
        float oldPen = weapon.armorPenetration;
        float oldRange = weapon.range;

        weapon.attack = Mathf.Min(99999, Mathf.RoundToInt(weapon.attack * 1.23f));
        weapon.critRate = Mathf.Min(100f, Mathf.RoundToInt(weapon.critRate * 1.23f));
        weapon.armorPenetration = Mathf.Min(9999, Mathf.RoundToInt(weapon.armorPenetration * 1.23f));
        weapon.range = Mathf.Min(999, Mathf.RoundToInt(weapon.range * 1.23f));

        currentStudent.OriginData.weaponStats = weapon;

        uiAnimator.PlayStatCounterAnimation(weaponAttackText, oldAttack, weapon.attack, true);
        uiAnimator.PlayStatCounterAnimation(weaponCritRateText, oldCrit, weapon.critRate, false);
        uiAnimator.PlayStatCounterAnimation(weaponPenetrationText, oldPen, weapon.armorPenetration, true);
        uiAnimator.PlayStatCounterAnimation(weaponRangeText, oldRange, weapon.range, true);

        UpdateWeaponUpgradePreviews();

        if (StudentListManager.Instance != null)
            StudentListManager.Instance.SortAndRefreshUI(false, false);
    }

    private void UpdateWeaponUpgradePreviews()
    {
        if (currentStudent == null) return;
        var w = currentStudent.OriginData.weaponStats;

        bool isMaxAttack = w.attack >= 99999;
        bool isMaxCrit = w.critRate >= 100f;
        bool isMaxPen = w.armorPenetration >= 9999;
        bool isMaxRange = w.range >= 999;

        nextWeaponAttackText.text = isMaxAttack ? "" : $"{Mathf.RoundToInt(w.attack * 1.23f)}";
        nextWeaponCritRateText.text = isMaxCrit ? "" : $"{(w.critRate * 1.23f):0}%";
        nextWeaponPenetrationText.text = isMaxPen ? "" : $"{Mathf.RoundToInt(w.armorPenetration * 1.23f)}";
        nextWeaponRangeText.text = isMaxRange ? "" : $"{Mathf.RoundToInt(w.range * 1.23f)}";
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

    // 메인 탭 (기본정보, 레벨업, 신비업그레이드) 토글
    public void OnClickMainTab(int index)
    {
        for (int i = 0; i < mainTabButtons.Length; i++)
        {
            if (mainTabButtons[i] != null)
            {
                mainTabButtons[i].sprite = (i == index) ? mainTabActiveSprite : mainTabInactiveSprite;
            }

            if (mainTabPanels[i] != null)
            {
                mainTabPanels[i].SetActive(i == index);
            }
        }
    }

    // 서브 탭 (스킬, 무기, 방어구) 토글
    public void OnClickSubTab(int index)
    {
        for (int i = 0; i < subTabButtons.Length; i++)
        {
            if (subTabButtons[i] != null)
            {
                subTabButtons[i].sprite = (i == index) ? subTabActiveSprite : subTabInactiveSprite;
            }

            if (subTabPanels[i] != null)
            {
                subTabPanels[i].SetActive(i == index);
            }
        }
    }

    public void OnClickLevelUpButton()
    {
        if (currentStudent == null || isExpAnimating || uiAnimator == null) return;

        float startExp = expSlider.value;
        float addExp = Random.Range(20, 151);
        int nextLevel = currentStudent.CurrentLevel + 1;

        isExpAnimating = true;

        uiAnimator.PlayExpGainAnimation(expSlider, levelText, startExp, addExp, nextLevel, () =>
        {
            float targetExp = startExp + addExp;
            if (targetExp >= expSlider.maxValue)
            {
                currentStudent.CurrentLevel += 1;
                currentStudent.OriginData.exp = (int)targetExp - (int)expSlider.maxValue;
            }
            else
            {
                currentStudent.OriginData.exp = (int)targetExp;
            }

            if (StudentListManager.Instance != null)
            {
                StudentListManager.Instance.SortAndRefreshUI(false, false);
            }
        });

        float totalDuration = (startExp + addExp >= expSlider.maxValue) ? 0.6f : 0.3f; 
        DOVirtual.DelayedCall(totalDuration, () => isExpAnimating = false);
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

        if (StudentSceneController.Instance != null)
        {
            StudentSceneController.Instance.currentState = UIState.StudentList;
        }
    }
}
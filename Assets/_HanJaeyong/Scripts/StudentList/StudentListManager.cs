using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public enum FilterType
{
    None,
    ALL,
    STRIKER,
    SPECIAL
}
public enum SortType
{
    LEVEL,
    NAME
}

public class StudentListManager : MonoBehaviour
{
    public static StudentListManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [Header("[보유학생들(SO넣기)]")]
    [SerializeField] private List<StudentData> studentSOList;

    [Header("[UI 연결]")]
    [SerializeField] private GameObject studentSlotPrefab;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private Image allTabImage;
    [SerializeField] private Image strikerTabImage;
    [SerializeField] private Image specialTabImage;
    [SerializeField] private Sprite allActiveSprite;
    [SerializeField] private Sprite allInactiveSprite;
    [SerializeField] private Sprite strikerActiveSprite;
    [SerializeField] private Sprite strikerInactiveSprite;
    [SerializeField] private Sprite specialActiveSprite;
    [SerializeField] private Sprite specialInactiveSprite;
    [SerializeField] private TextMeshProUGUI sortButtonText;
    [SerializeField] private RectTransform sortDirectionArrowRect;

    [Header("매니저")]
    [SerializeField] private UIAnimator uiAnimator;

    private FilterType currentFilter = FilterType.None;
    private SortType currentSortType = SortType.LEVEL;
    private bool isDescending = true;

    private List<Student> runtimeStudentList = new List<Student>();
    public List<Student> CurrentActiveList { get; private set; } = new List<Student>();

    private Dictionary<Student, StudentSlotUI> slotUIDict = new Dictionary<Student, StudentSlotUI>();


    void Start()
    {
        InitializeData();
        CreateUISlots();

        SetFilter(FilterType.ALL, true);
    }

    private void InitializeData()
    {
        foreach (var so in studentSOList)
        {
            if (so != null)
            {
                runtimeStudentList.Add(new Student(so));
            }
        }
    }

    private void CreateUISlots()
    {
        // 테스트 슬롯들 예외처리
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (var student in runtimeStudentList)
        {
            GameObject go = Instantiate(studentSlotPrefab, contentTransform);
            StudentSlotUI slotUI = go.GetComponent<StudentSlotUI>();

            if (slotUI != null)
            {
                slotUI.Setup(student, this, uiAnimator);

                slotUIDict.Add(student, slotUI);
            }
        }
    }

    public void OnClickTabAll() { SetFilter(FilterType.ALL, true); }
    public void OnClickTabStriker() { SetFilter(FilterType.STRIKER, true); }
    public void OnClickTabSpecial() { SetFilter(FilterType.SPECIAL, true); }

    private void SetFilter(FilterType newFilter, bool playAnim = false)
    {
        if (currentFilter == newFilter) return;

        currentFilter = newFilter;

        UpdateTabVisuals();

        SortAndRefreshUI(playAnim);
    }

    private void UpdateTabVisuals()
    {
        allTabImage.sprite =        (currentFilter == FilterType.ALL)       ? allActiveSprite       : allInactiveSprite;

        strikerTabImage.sprite =    (currentFilter == FilterType.STRIKER)   ? strikerActiveSprite   : strikerInactiveSprite;

        specialTabImage.sprite =    (currentFilter == FilterType.SPECIAL)   ? specialActiveSprite   : specialInactiveSprite;
    }

    public void OnClickSortToggle()
    {
        if (currentSortType == SortType.LEVEL)
        {
            currentSortType = SortType.NAME;
            sortButtonText.text = "A~Z";
        }
        else
        {
            currentSortType = SortType.LEVEL;
            sortButtonText.text = "Lv";
        }

        SortAndRefreshUI(true);
    }
    public void OnClickDirectionToggle()
    {
        isDescending = !isDescending;

        //Y축 스케일 변경으로 이미지 표시
        float targetScaleY = isDescending ? 1f : -1f;

        if (sortDirectionArrowRect != null)
        {
            Vector3 currentScale = sortDirectionArrowRect.localScale;
            sortDirectionArrowRect.localScale = new Vector3(currentScale.x, targetScaleY, currentScale.z);
        }

        SortAndRefreshUI(true);
    }

    public void SortAndRefreshUI(bool playPopInAnim = false, bool playReorderAnim = false)
    {

        //정렬 애니메이션을 위한 기존 위치 저장
        Dictionary<Transform, Vector3> oldLocalPositions = new Dictionary<Transform, Vector3>();
        if (playReorderAnim)
        {
            foreach (Transform child in contentTransform)
            {
                if (child.gameObject.activeSelf)
                {
                    oldLocalPositions[child] = child.localPosition;
                }
            }
        }

        CurrentActiveList.Clear();

        // 1순위 정렬: 즐겨찾기(내림차순 고정)
        IOrderedEnumerable<Student> query = runtimeStudentList.OrderByDescending(s => s.IsFavorite);

        // 2순위 정렬: 레벨/이름, 내림/오름차순
        if (currentSortType == SortType.LEVEL)
        {
            if (isDescending)
            {
                // 레벨 내림차순 (높은 레벨 -> 낮은 레벨)
                query = query.ThenByDescending(s => s.CurrentLevel);
            }
            else
            {
                // 레벨 오름차순 (낮은 레벨 -> 높은 레벨)
                query = query.ThenBy(s => s.CurrentLevel);
            }
        }
        else if (currentSortType == SortType.NAME)
        {
            if (isDescending)
            {
                // 이름 내림차순 (하 -> 가 / Z -> A)
                query = query.ThenByDescending(s => s.OriginData.studentName);
            }
            else
            {
                // 이름 오름차순 (가 -> 하 / A -> Z)
                query = query.ThenBy(s => s.OriginData.studentName);
            }
        }

        var sortedList = query.ToList();

        List<GameObject> activeSlotsForAnim = new List<GameObject>();

        //필터타입에 따라 보여줄애들 보여주기
        foreach (var student in sortedList)
        {
            if (slotUIDict.TryGetValue(student, out StudentSlotUI slotUI))
            {
                bool isMatch = (currentFilter == FilterType.ALL) ||
                               (currentFilter == FilterType.STRIKER && student.OriginData.role == StudentRole.STRIKER) ||
                               (currentFilter == FilterType.SPECIAL && student.OriginData.role == StudentRole.SPECIAL);

                slotUI.gameObject.SetActive(isMatch);

                if (isMatch)
                {
                    slotUI.transform.SetAsLastSibling();
                    slotUI.UpdateUI();

                    activeSlotsForAnim.Add(slotUI.gameObject);

                    CurrentActiveList.Add(student);
                }
            }
        }

        if (playPopInAnim && uiAnimator != null)
        {
            // 재정렬 슬롯 생성 연출
            uiAnimator.PlayListPopIn(activeSlotsForAnim);
        }
        else if (playReorderAnim && uiAnimator != null)
        {
            // 자리이동 연출
            uiAnimator.PlayLayoutReorder(oldLocalPositions, contentTransform as RectTransform);
        }
    }//SortAndRefreshUI------End.



}
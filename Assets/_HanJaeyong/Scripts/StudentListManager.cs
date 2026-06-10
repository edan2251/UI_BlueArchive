using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StudentListManager : MonoBehaviour
{
    [Header("[보유학생들(SO넣기)]")]
    [SerializeField] private List<StudentData> studentSOList;

    [Header("[UI 연결]")]
    [SerializeField] private GameObject studentSlotPrefab;
    [SerializeField] private Transform contentTransform;

    private List<Student> runtimeStudentList = new List<Student>();

    private Dictionary<Student, StudentSlotUI> slotUIDict = new Dictionary<Student, StudentSlotUI>();

    void Start()
    {
        InitializeData();
        CreateUISlots();
        SortAndRefreshUI();
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
                slotUI.Setup(student, this);

                slotUIDict.Add(student, slotUI);
            }
        }
    }

    public void SortAndRefreshUI()
    {
        var sortedList = runtimeStudentList
            .OrderByDescending(s => s.IsFavorite)
            .ThenBy(s => s.OriginData.studentName)
            .ToList();

        foreach (var student in sortedList)
        {
            if (slotUIDict.TryGetValue(student, out StudentSlotUI slotUI))
            {
                slotUI.transform.SetAsLastSibling();
                slotUI.UpdateUI();
            }
        }
    }
}
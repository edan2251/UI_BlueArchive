using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StudentSlotUI : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Image favoritePinImage;

    private Student myStudent;
    private StudentListManager manager;

    public void Setup(Student student, StudentListManager listManager)
    {
        myStudent = student;
        manager = listManager;
        UpdateUI();
    }

    public void UpdateUI()
    {
        nameText.text = myStudent.OriginData.studentName;
        levelText.text = $"Lv.{myStudent.CurrentLevel}";

        if(favoritePinImage != null)
        {
            if(myStudent.IsFavorite)
            {
                favoritePinImage.color = Color.darkOliveGreen;
            }
            else
            {
                favoritePinImage.color = Color.white;
            }
        }
    }

    // 핀버튼클릭시 호출
    public void OnClickFavoriteToggle()
    {
        myStudent.ToggleFavorite();

        if (manager != null)
        {
            manager.SortAndRefreshUI();
        }
    }

    public void OnClickDetail()
    {
        GameManager.SelectedStudent = myStudent;
        SceneManager.LoadScene("Student");
    }
}
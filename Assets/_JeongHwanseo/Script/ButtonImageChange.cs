using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChanger : MonoBehaviour
{
    [Header("클릭했을 때 부모 버튼에 영구적으로 보여줄 새로운 소스 이미지")]
    public Sprite nextSprite;

    private Button parentButton;   // 부모의 버튼 컴포넌트
    private Image parentImage;     // 부모의 이미지 컴포넌트 (우리가 바꿀 대상!)
    private bool isClicked = false; // 한 번만 클릭되게 하는 플래그

    void Awake()
    {
        // 1. 내 부모(Parent) 오브젝트에서 Button과 Image 컴포넌트를 모두 찾습니다.
        parentButton = GetComponentInParent<Button>();
        parentImage = GetComponentInParent<Image>();

        if (parentImage == null)
        {
            Debug.LogError("부모 오브젝트에서 Image 컴포넌트를 찾을 수 없습니다! 구조를 확인해주세요.");
        }

        // 2. 부모 버튼의 클릭 이벤트에 내 함수를 자동으로 연결합니다.
        if (parentButton != null)
        {
            parentButton.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("부모 오브젝트에서 Button 컴포넌트를 찾을 수 없습니다!");
        }
    }

    private void OnButtonClick()
    {
        // 이미 한 번 클릭되었다면 중복 실행 방지
        if (isClicked) return;

        if (parentImage != null && nextSprite != null)
        {
            // [핵심] 부모의 소스 이미지를 새로운 이미지로 변경하고, 끝!
            parentImage.sprite = nextSprite;
            isClicked = true; // 이제 더 이상 클릭되지 않도록 설정

            // (선택사항) 이미지가 바뀐 뒤에는 버튼 기능 자체를 아예 끄고 싶다면 아래 줄을 주석 해제하세요.
            // parentButton.interactable = false; 
        }
        else
        {
            Debug.LogWarning("부모 Image 컴포넌트나 Next Sprite가 비어있습니다!");
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChanger : MonoBehaviour
{
    [Header("클릭했을 때 부모 버튼에 영구적으로 보여줄 새로운 소스 이미지")]
    public Sprite nextSprite;

    [Header("재생할 오디오 클립")]
    public AudioClip responseSound; // 재생할 효과음 파일

    private Button parentButton;   // 부모의 버튼 컴포넌트
    private Image parentImage;     // 부모의 이미지 컴포넌트
    private AudioSource audioSource; // 사운드 재생을 위한 컴포넌트
    private bool isClicked = false; // 한 번만 클릭되게 하는 플래그

    void Awake()
    {
        // 1. 컴포넌트들을 찾습니다.
        parentButton = GetComponentInParent<Button>();
        parentImage = GetComponentInParent<Image>();

        // 사운드 재생을 위해 본인 오브젝트(또는 부모)에서 AudioSource를 가져옵니다.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 만약 오브젝트에 AudioSource가 없다면 자동으로 추가해 줍니다.
            audioSource = gameObject.AddComponent<AudioSource>();
        }

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
            // [핵심] 부모의 소스 이미지를 새로운 이미지로 변경
            parentImage.sprite = nextSprite;
            isClicked = true; // 이제 더 이상 클릭되지 않도록 설정

            // [추가] 사운드를 2.5초간 재생하는 코루틴 시작
            if (audioSource != null && responseSound != null)
            {
                StartCoroutine(PlaySoundForDuration(2.5f));
            }

            // (선택사항) 이미지가 바뀐 뒤에는 버튼 기능 자체를 아예 끄고 싶다면 아래 줄을 주석 해제하세요.
            // parentButton.interactable = false; 
        }
        else
        {
            Debug.LogWarning("부모 Image 컴포넌트나 Next Sprite가 비어있습니다!");
        }
    }

    // 지정한 시간(초) 동안만 사운드를 재생하고 정지하는 코루틴
    private IEnumerator PlaySoundForDuration(float duration)
    {
        audioSource.clip = responseSound;
        audioSource.Play();

        // 매개변수로 받은 duration(2.5초)만큼 기다립니다.
        yield return new WaitForSeconds(duration);

        // 시간이 지나면 사운드를 부드럽게 끄거나 즉시 정지합니다.
        audioSource.Stop();
    }
}
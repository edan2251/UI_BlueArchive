using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 오디오 재생을 위해 오디오 소스 컴포넌트가 없다면 자동으로 추가해줍니다.
[RequireComponent(typeof(AudioSource))]
public class ImageSpawner : MonoBehaviour
{
    [Header("생성할 이미지 (UI Image 프리팹 또는 씬에 숨겨둔 오브젝트)")]
    public GameObject imagePrefab;

    [Header("이미지가 나타날 부모 위치 (Canvas 내부 권장)")]
    public Transform spawnParent;

    [Header("이미지가 나타날 정확한 좌표")]
    public Vector3 spawnPosition;

    [Header("2.5초 뒤에 바뀔 새로운 소스 이미지(Sprite)")]
    public Sprite nextSprite;

    [Header("사운드 설정 (오디오 클립)")]
    public AudioClip firstSound;  // 이미지 생성 시 즉시 재생할 첫 번째 사운드
    public AudioClip secondSound; // 첫 번째 사운드가 끝나면 재생할 두 번째 사운드

    private AudioSource audioSource;

    void Awake()
    {
        // 내 오브젝트에 있는 오디오 소스 컴포넌트를 가져옵니다.
        audioSource = GetComponent<AudioSource>();
    }

    // 버튼이 클릭되었을 때 호출할 함수
    public void OnButtonClick(Button clickedButton)
    {
        // 1. 즉시 버튼 비활성화 및 삭제
        clickedButton.interactable = false;
        Destroy(clickedButton.gameObject);

        // 2. 전체 시퀀스를 제어하는 코루틴 시작
        StartCoroutine(SpawnAndAudioSequence());
    }

    private IEnumerator SpawnAndAudioSequence()
    {
        // [단계 1] 버튼 삭제 후 1초 대기
        yield return new WaitForSeconds(1f);

        GameObject spawnedImage = null;

        // 이미지 생성
        if (imagePrefab != null)
        {
            spawnedImage = Instantiate(imagePrefab, spawnParent);
            spawnedImage.transform.localPosition = spawnPosition;
        }
        else
        {
            Debug.LogWarning("지정된 이미지 프리팹이 없습니다!");
            yield break; // 이미지가 없으면 사운드도 재생하지 않고 종료
        }

        // [단계 2] 이미지 생성 직후 첫 번째 사운드 재생 및 대기
        if (audioSource != null && firstSound != null)
        {
            audioSource.PlayOneShot(firstSound);

            // 첫 번째 사운드가 재생되는 시간(초)만큼 정확하게 대기합니다.
            yield return new WaitForSeconds(firstSound.length);
        }

        // [단계 3] 첫 번째 사운드가 끝나자마자 두 번째 사운드 재생
        if (audioSource != null && secondSound != null)
        {
            audioSource.PlayOneShot(secondSound);
        }

        // [단계 4] 이미지 생성 시점 기준으로 총 2.5초를 맞춰야 하므로 계산 후 대기
        // (주의: 첫 번째 사운드 재생 시간만큼 이미 시간을 소모했으므로 남은 시간만 기다립니다)
        float elapsedSoundTime = firstSound != null ? firstSound.length : 0f;
        float remainingTimeForImageChange = 2.5f - elapsedSoundTime;

        // 만약 사운드가 2.5초보다 길다면 즉시 변경하고, 남은 시간이 있다면 그만큼 기다립니다.
        if (remainingTimeForImageChange > 0f)
        {
            yield return new WaitForSeconds(remainingTimeForImageChange);
        }

        // [단계 5] 이미지 소스 변경
        if (spawnedImage != null && nextSprite != null)
        {
            Image uiiImage = spawnedImage.GetComponent<Image>();
            if (uiiImage != null)
            {
                uiiImage.sprite = nextSprite;
            }
        }
    }
}
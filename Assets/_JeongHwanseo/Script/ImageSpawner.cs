using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트를 제어하기 위해 필수!

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

    // 버튼이 클릭되었을 때 호출할 함수
    public void OnButtonClick(Button clickedButton)
    {
        // 1. 즉시 버튼 비활성화 및 삭제
        clickedButton.interactable = false; // 중복 클릭 방지
        Destroy(clickedButton.gameObject);

        // 2. 시간차 제어를 위한 코루틴 시작
        StartCoroutine(SpawnAndChangeImageSequence());
    }

    private IEnumerator SpawnAndChangeImageSequence()
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
            yield break; // 프리팹이 없으면 코루틴 종료
        }

        // [단계 2] 이미지 생성 후 다시 2.5초 대기
        yield return new WaitForSeconds(2.5f);

        // [단계 3] 생성된 오브젝트에서 Image 컴포넌트를 찾아 소스 이미지 변경
        if (spawnedImage != null && nextSprite != null)
        {
            Image uiiImage = spawnedImage.GetComponent<Image>();

            if (uiiImage != null)
            {
                uiiImage.sprite = nextSprite;
            }
            else
            {
                Debug.LogWarning("생성된 오브젝트에 UI 'Image' 컴포넌트가 없습니다. UI 이미지가 맞는지 확인해주세요!");
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSequenceManager : MonoBehaviour
{
    [Header("스크롤 뷰의 Content 트랜스폼 (자식 삭제용)")]
    public Transform scrollViewContent;

    [Header("활성화할 메인 패널 오브젝트")]
    public GameObject panelObject;

    [Header("패널이 열린 후 나타날 또 다른 오브젝트들")]
    public List<GameObject> secondaryObjects = new List<GameObject>();

    private Vector3 panelOriginalScale;
    private Dictionary<GameObject, Vector3> secondaryOriginalScales = new Dictionary<GameObject, Vector3>();
    private bool isRunning = false; // 연출 도중 중복 클릭 방지

    void Awake()
    {
        // 원래 에디터에 세팅된 오리지널 스케일들을 기억해두고 꺼둡니다.
        if (panelObject != null)
        {
            panelOriginalScale = panelObject.transform.localScale;
            panelObject.SetActive(false);
        }

        foreach (var obj in secondaryObjects)
        {
            if (obj != null)
            {
                secondaryOriginalScales[obj] = obj.transform.localScale;
                obj.SetActive(false);
            }
        }
    }

    // 버튼과 연결할 클릭 함수
    public void OnButtonClick()
    {
        if (isRunning) return; // 이미 연출이 진행 중이면 무시
        StartCoroutine(ExecuteSequence());
    }

    private IEnumerator ExecuteSequence()
    {
        isRunning = true;

        // [단계 1] 스크롤 뷰 Content의 모든 자식 오브젝트 삭제
        if (scrollViewContent != null)
        {
            foreach (Transform child in scrollViewContent)
            {
                Destroy(child.gameObject);
            }
        }

        // [단계 2] 메인 패널 활성화 및 1초 동안 등장 (0.1 -> 2.2 -> 기존 Scale)
        if (panelObject != null)
        {
            panelObject.SetActive(true);
            Transform panelTx = panelObject.transform;

            float duration = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = elapsed / duration;

                Vector3 currentScale;
                if (normalizedTime < 0.5f)
                {
                    float t = normalizedTime * 2f;
                    currentScale = Vector3.Lerp(panelOriginalScale * 0.1f, panelOriginalScale * 1.5f, t);
                }
                else
                {
                    float t = (normalizedTime - 0.5f) * 2f;
                    currentScale = Vector3.Lerp(panelOriginalScale * 1.5f, panelOriginalScale, t);
                }

                panelTx.localScale = currentScale;
                yield return null;
            }
            panelTx.localScale = panelOriginalScale;
        }

        // [단계 3] 서브 오브젝트들 활성화 및 1초 동안 등장 (0.1 -> 기존 Scale)
        if (secondaryObjects.Count > 0)
        {
            foreach (var obj in secondaryObjects)
            {
                if (obj != null)
                {
                    Vector3 origScale = secondaryOriginalScales[obj];
                    obj.transform.localScale = origScale * 0.1f;
                    obj.SetActive(true);
                }
            }

            float duration = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;

                foreach (var obj in secondaryObjects)
                {
                    if (obj != null)
                    {
                        Vector3 origScale = secondaryOriginalScales[obj];
                        obj.transform.localScale = Vector3.Lerp(origScale * 0.1f, origScale, t);
                    }
                }
                yield return null;
            }

            foreach (var obj in secondaryObjects)
            {
                if (obj != null)
                {
                    obj.transform.localScale = secondaryOriginalScales[obj];
                }
            }
        }

        // [새로 추가된 단계 4] 모든 등장이 끝난 후 "5초 동안 대기"
        yield return new WaitForSeconds(5f);

        // [새로 추가된 단계 5] 0.5초 동안 서브 오브젝트들과 메인 패널을 부드럽게 축소시키며 비활성화
        float fadeOutDuration = 0.5f;
        float fadeOutElapsed = 0f;

        while (fadeOutElapsed < fadeOutDuration)
        {
            fadeOutElapsed += Time.deltaTime;
            float t = fadeOutElapsed / fadeOutDuration;

            // 서브 오브젝트들 축소 (기존 -> 0.1)
            foreach (var obj in secondaryObjects)
            {
                if (obj != null)
                {
                    Vector3 origScale = secondaryOriginalScales[obj];
                    obj.transform.localScale = Vector3.Lerp(origScale, origScale * 0.1f, t);
                }
            }

            // 메인 패널 축소 (기존 -> 0.1)
            if (panelObject != null)
            {
                panelObject.transform.localScale = Vector3.Lerp(panelOriginalScale, panelOriginalScale * 0.1f, t);
            }

            yield return null;
        }

        // 완전히 작아진 후 최종적으로 오브젝트들을 꺼줍니다 (SetActive false)
        if (panelObject != null) panelObject.SetActive(false);
        foreach (var obj in secondaryObjects)
        {
            if (obj != null) obj.SetActive(false);
        }

        isRunning = false; // 이제 다음 클릭을 받을 수 있도록 플래그 해제
    }
}
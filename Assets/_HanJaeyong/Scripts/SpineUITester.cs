using UnityEngine;
using Spine.Unity;

public class SpineUITester : MonoBehaviour
{
    [Header("[테스트할 캐릭터 스파인 UI]")]
    [SerializeField] private SkeletonGraphic spineCharacter;

    void Update()
    {
        if (spineCharacter == null || spineCharacter.AnimationState == null) return;

        // 숫자 1번: 기본 대기 (반복 O)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spineCharacter.AnimationState.SetAnimation(0, "stand A", true);
            Debug.Log("1번: stand A 재생");
        }

        // 숫자 2번: 스킬 사용 (반복 X)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spineCharacter.AnimationState.SetAnimation(0, "skill A", false);

            spineCharacter.AnimationState.AddAnimation(0, "stand A", true, 0f);
            Debug.Log("2번: skill A 재생 후 stand A 복귀");
        }

        // 숫자 3번: 화남 (반복 O)
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spineCharacter.AnimationState.SetAnimation(0, "stand(angry)", true);
            Debug.Log("3번: stand(angry) 재생");
        }

        // 숫자 4번: 웃음 (반복 O)
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            spineCharacter.AnimationState.SetAnimation(0, "stand(laugh)", true);
            Debug.Log("4번: stand(laugh) 재생");
        }

        // 숫자 5번: 피격 (반복 X, 맞고 나서 다시 기본 대기로 돌아가게끔 예약 연결)
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // SetAnimation은 즉시 재생
            spineCharacter.AnimationState.SetAnimation(0, "stand(hit)", false);

            spineCharacter.AnimationState.AddAnimation(0, "stand A", true, 0f);

            Debug.Log("5번: stand(hit) 재생 후 stand A 복귀");
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using Spine.Unity;

public class SpineClickInteraction : MonoBehaviour, IPointerClickHandler
{
    private SkeletonGraphic spineCharacter;
    [SerializeField] private string defaultAnimation = "stand A";

    [SerializeField]
    private string[] animationNames = new string[]
    {
        "skill A",
        "stand(angry)",
        "stand(laugh)",
        "stand(sad)",
        "stand(shy)",
        "stand(smile)",
        "stand(surpriseA)",
        "stand(surpriseB)",
        "stand A"
    };

    private void Awake()
    {
        spineCharacter = GetComponent<SkeletonGraphic>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (spineCharacter == null || spineCharacter.AnimationState == null || animationNames.Length == 0) return;

        int randomIndex = Random.Range(0, animationNames.Length);
        string selectedAnim = animationNames[randomIndex];

        spineCharacter.AnimationState.SetAnimation(0, selectedAnim, false);

        if (selectedAnim != defaultAnimation)
        {
            spineCharacter.AnimationState.AddAnimation(0, defaultAnimation, true, 0f);
        }
    }
}
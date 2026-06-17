using UnityEngine;
using UnityEngine.InputSystem;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance;

    public GameObject uiParticlePrefab;

    public Transform globalCanvasTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SpawnParticle(Mouse.current.position.ReadValue());
        }
    }

    private void SpawnParticle(Vector2 screenPos)
    {
        if (uiParticlePrefab == null || globalCanvasTransform == null) return;

        GameObject effect = Instantiate(uiParticlePrefab, globalCanvasTransform);

        RectTransform canvasRect = globalCanvasTransform.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out Vector2 localPoint))
        {
            effect.transform.localPosition = localPoint;
        }

        effect.transform.SetAsLastSibling();
    }
}
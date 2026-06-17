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
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];
            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                SpawnParticle(touch.position.ReadValue());
                return;
            }
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
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
            effect.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
        }

        effect.transform.SetAsLastSibling();
    }
}
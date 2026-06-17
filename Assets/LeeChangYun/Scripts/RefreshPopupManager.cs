using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 

public class RefreshPopupManager : MonoBehaviour
{
    public static RefreshPopupManager instance;

    [Header("Popup Animation")]
    public GameObject popupPanel;
    public CanvasGroup canvasGroup; 
    public RectTransform popupBox;  

    [Header("Settings")]
    public int refreshCost = 40;
    public TextMeshProUGUI priceText;

    [Header("Buttons")]
    public Button confirmButton;
    public Button cancelButton;

    public ShopManager shopManager;

    private void Awake()
    {
        if (instance == null) instance = this;
        popupPanel.SetActive(false);
    }

    private void Start()
    {
        if (confirmButton != null) confirmButton.onClick.AddListener(ConfirmRefresh);
        if (cancelButton != null) cancelButton.onClick.AddListener(ClosePopup);

        if (priceText != null) priceText.text = refreshCost.ToString();
    }

    public void OpenPopup()
    {
        popupPanel.SetActive(true);


        if (canvasGroup != null && popupBox != null)
        {
            canvasGroup.DOKill();
            popupBox.DOKill();

            canvasGroup.alpha = 0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            popupBox.localScale = Vector3.zero;

            canvasGroup.DOFade(1f, 0.25f);
            popupBox.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        }
    }

    private void ClosePopup()
    {
        if (canvasGroup != null && popupBox != null)
        {
            canvasGroup.DOKill();
            popupBox.DOKill();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(popupBox.DOScale(0f, 0.2f).SetEase(Ease.InBack));
            sequence.Join(canvasGroup.DOFade(0f, 0.2f));

            sequence.OnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                popupPanel.SetActive(false);
            });
        }
        else
        {
            popupPanel.SetActive(false);
        }
    }

    private void ConfirmRefresh()
    {
        if (CurrencyManager.instance.SpendDiamond(refreshCost))
        {
            if (shopManager != null)
            {
                shopManager.ResetAllPurchaseCounts();
            }
            ClosePopup(); 
        }
        else
        {
            Debug.Log("다이아가 부족하여 갱신할 수 없습니다.");
        }
    }
}
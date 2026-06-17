using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; 

public class PurchasePopupManager : MonoBehaviour
{
    public static PurchasePopupManager instance;
    public GameObject popupPanel;
    public CanvasGroup canvasGroup;
    public RectTransform popupBox;

    public Image dimmedImage;

    [Header("Item Info")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemOwnedQuantityText;

    [Header("Price Info")]
    public TextMeshProUGUI priceText;
    public Image priceCurrencyIcon;

    [Header("My Currency Info")]
    public TextMeshProUGUI myCurrencyText;
    public Image myCurrencyIcon;

    [Header("Resources")]
    public Sprite coinSprite;
    public Sprite diamondSprite;

    [Header("Buttons")]
    public Button confirmButton;
    public Button cancelButton;

    private ShopItem currentSlot;

    private void Awake()
    {
        if (instance == null) instance = this;
        popupPanel.SetActive(false);
    }

    private void Start()
    {
        if (confirmButton != null) confirmButton.onClick.AddListener(ConfirmPurchase);
        if (cancelButton != null) cancelButton.onClick.AddListener(ClosePopup);
    }

    public void OpenPopup(ShopItem slot)
    {
        currentSlot = slot;
        ItemData item = slot.itemData;

        if (itemIcon != null) itemIcon.sprite = item.itemIcon;
        if (itemNameText != null) itemNameText.text = item.itemName;
        if (itemOwnedQuantityText != null) itemOwnedQuantityText.text = "보유 수량 " + item.ownedQuantity.ToString();
        if (priceText != null) priceText.text = item.itemPrice.ToString("N0");

        bool canAfford = false;

        if (item.currencyType == CurrencyType.Coin)
        {
            if (myCurrencyText != null) myCurrencyText.text = CurrencyManager.instance.coins.ToString("N0");
            if (myCurrencyIcon != null) myCurrencyIcon.sprite = coinSprite;
            if (priceCurrencyIcon != null) priceCurrencyIcon.sprite = coinSprite;
            canAfford = CurrencyManager.instance.coins >= item.itemPrice;
        }
        else if (item.currencyType == CurrencyType.Diamond)
        {
            if (myCurrencyText != null) myCurrencyText.text = CurrencyManager.instance.diamonds.ToString("N0");
            if (myCurrencyIcon != null) myCurrencyIcon.sprite = diamondSprite;
            if (priceCurrencyIcon != null) priceCurrencyIcon.sprite = diamondSprite;
            canAfford = CurrencyManager.instance.diamonds >= item.itemPrice;
        }

        if (confirmButton != null)
        {
            Image btnImage = confirmButton.GetComponent<Image>();
            if (canAfford)
            {
                confirmButton.interactable = true;
                if (btnImage != null) btnImage.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                confirmButton.interactable = false;
                if (btnImage != null) btnImage.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

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

            if (dimmedImage != null)
            {
                dimmedImage.gameObject.SetActive(true);
                dimmedImage.color = new Color(0, 0, 0, 0);
                dimmedImage.DOFade(0.5f, 0.25f);          
            }
        }
    }

    private void ClosePopup()
    {
        if (canvasGroup != null && popupBox != null)
        {
            canvasGroup.DOKill();
            popupBox.DOKill();

            Sequence sequence = DOTween.Sequence();

            if (dimmedImage != null)
            {
                sequence.Join(dimmedImage.DOFade(0f, 0.2f));
            }

            sequence.Append(popupBox.DOScale(0f, 0.2f).SetEase(Ease.InBack));
            sequence.Join(canvasGroup.DOFade(0f, 0.2f));

            sequence.OnComplete(() =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                popupPanel.SetActive(false);
                if (dimmedImage != null) dimmedImage.gameObject.SetActive(false);
            });
        }
        else
        {
            popupPanel.SetActive(false);
        }
    }

    private void ConfirmPurchase()
    {
        ItemData item = currentSlot.itemData;
        bool isSuccess = false;

        if (item.currencyType == CurrencyType.Coin)
        {
            isSuccess = CurrencyManager.instance.SpendCoin(item.itemPrice);
        }
        else if (item.currencyType == CurrencyType.Diamond)
        {
            isSuccess = CurrencyManager.instance.SpendDiamond(item.itemPrice);
        }

        if (isSuccess)
        {
            item.ownedQuantity++;
            item.currentPurchaseCount++;
            currentSlot.UpdateSlotUI();
            ClosePopup(); 
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public ItemData itemData;

    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public Image iconImage;
    public TextMeshProUGUI priceText;
    public Button buyButton;
    public Image currencyIcon;
    public Sprite coinSprite;
    public Sprite diamondSprite;

    [Header("Limit System")]
    public TextMeshProUGUI limitText;
    public CanvasGroup canvasGroup;     

    public void InitializeItem()
    {
        if (itemData == null) return;

        if (nameText != null) nameText.text = itemData.itemName;
        if (iconImage != null) iconImage.sprite = itemData.itemIcon;
        if (priceText != null) priceText.text = itemData.itemPrice.ToString("N0");

        if (currencyIcon != null)
        {
            currencyIcon.sprite = (itemData.currencyType == CurrencyType.Coin) ? coinSprite : diamondSprite;
        }

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnClickBuyButton);
        }

        UpdateSlotUI();
    }


    public void UpdateSlotUI()
    {
        int remainCount = itemData.maxPurchaseLimit - itemData.currentPurchaseCount;

        if (limitText != null)
        {
            limitText.text = $"남은 수량: {remainCount}";
        }

        // 품절 처리 로직
        if (remainCount <= 0)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0.5f;  
                canvasGroup.interactable = false;    
                canvasGroup.blocksRaycasts = false;   
            }
        }
        else
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1.0f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }

    private void OnClickBuyButton()
    {
        PurchasePopupManager.instance.OpenPopup(this);
    }
}
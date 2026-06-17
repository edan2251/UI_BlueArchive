using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchasePopupManager : MonoBehaviour
{
    public static PurchasePopupManager instance;

    public GameObject popupPanel;

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

    private ShopItem currentSlot; // 누른 슬롯 자체를 기억

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
    }

    private void ClosePopup()
    {
        popupPanel.SetActive(false);
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
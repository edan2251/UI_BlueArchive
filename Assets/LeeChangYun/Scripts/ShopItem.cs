using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public ItemData itemData;

    public TextMeshProUGUI nameText;
    public Image iconImage;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private void Start()
    {
        InitializeItem();
    }

    public void InitializeItem()
    {
        if (itemData == null) return;

        if (nameText != null) nameText.text = itemData.itemName;
        if (iconImage != null) iconImage.sprite = itemData.itemIcon;
        if (priceText != null) priceText.text = itemData.itemPrice.ToString("N0");

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyItem);
        }
    }

    private void BuyItem()
    {
        if (CurrencyManager.instance.SpendCoin(itemData.itemPrice))
        {
            Debug.Log($"{itemData.itemName} 구매 성공!");
        }
        else
        {
            Debug.Log("코인이 부족합니다.");
        }
    }
}
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

    public Image currencyIcon;
    public Sprite coinSprite;
    public Sprite diamondSprite;

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

        if (currencyIcon != null)
        {
            currencyIcon.sprite = (itemData.currencyType == CurrencyType.Coin) ? coinSprite : diamondSprite;
        }

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyItem);
        }
    }

    private void BuyItem()
    {
        bool isSuccess = false;

        if (itemData.currencyType == CurrencyType.Coin)
        {
            isSuccess = CurrencyManager.instance.SpendCoin(itemData.itemPrice);
        }
        else if (itemData.currencyType == CurrencyType.Diamond)
        {
            isSuccess = CurrencyManager.instance.SpendDiamond(itemData.itemPrice);
        }

        if (isSuccess)
        {
            Debug.Log($"{itemData.itemName} 구매 성공!");
        }
        else
        {
            Debug.Log("재화가 부족합니다.");
        }
    }
}
using UnityEngine;

public enum CurrencyType
{
    Coin,
    Diamond
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Shop/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public int itemPrice;
    public CurrencyType currencyType;
}
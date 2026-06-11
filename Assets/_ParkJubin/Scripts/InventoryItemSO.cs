using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InventoryItemSO", menuName = "Scriptable Objects/InventoryItemSO")]
public class InventoryItemSO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Image itemImage;
    public string itemType;
    public bool ableToUse;
}

using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform contentPanel;
    public List<ItemData> shopItems;

    private void Start()
    {
        GenerateShopItems();
    }

    private void GenerateShopItems()
    {
        foreach (ItemData item in shopItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentPanel);
            ShopItem shopItemScript = newSlot.GetComponent<ShopItem>();

            if (shopItemScript != null)
            {
                shopItemScript.itemData = item;
                shopItemScript.InitializeItem();
            }
        }
    }
}
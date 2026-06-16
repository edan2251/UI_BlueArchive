using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ShopTab
{
    public Button tabButton;
    public List<ItemData> tabItems;
    public Sprite normalSprite;
    public Sprite selectedSprite;
}

public class ShopManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform contentPanel;
    public List<ShopTab> shopTabs;

    private void Start()
    {
        for (int i = 0; i < shopTabs.Count; i++)
        {
            int index = i;
            shopTabs[i].tabButton.onClick.AddListener(() => ChangeTab(index));
        }

        if (shopTabs.Count > 0)
        {
            ChangeTab(0);
        }
    }

    public void ChangeTab(int tabIndex)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < shopTabs.Count; i++)
        {
            Image btnImage = shopTabs[i].tabButton.GetComponent<Image>();
            if (btnImage != null)
            {
                btnImage.sprite = (i == tabIndex) ? shopTabs[i].selectedSprite : shopTabs[i].normalSprite;
            }
        }

        List<ItemData> itemsToLoad = shopTabs[tabIndex].tabItems;

        foreach (ItemData item in itemsToLoad)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentPanel);
            ShopItem shopItemScript = newSlot.GetComponent<ShopItem>();

            if (shopItemScript != null)
            {
                shopItemScript.itemData = item;
                shopItemScript.InitializeItem();
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentPanel.GetComponent<RectTransform>());
    }
}
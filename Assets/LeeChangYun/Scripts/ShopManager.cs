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

    private void Awake()
    {
        // 게임을 켤 때마다 모든 아이템의 정보 초기화 (에디터 저장 방지)
        foreach (ShopTab tab in shopTabs)
        {
            foreach (ItemData item in tab.tabItems)
            {
                if (item != null)
                {
                    item.ownedQuantity = 0;
                    item.currentPurchaseCount = 0;
                }
            }
        }
    }

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
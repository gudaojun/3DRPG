using UnityEngine;
using UnityEngine.UI;
public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public Text amount = null;

    public ItemData_SO currentItemData;

    public InventoryData_SO Bag { get; set; }
    public int Index { get; set; } = -1;
    public void SetupItemUI(ItemData_SO itemData, int itemAmount)
    {
        if (itemAmount == 0)
        {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }

        if (itemAmount < 0)
            itemData = null;

        if (itemData != null)
        {
            currentItemData = itemData;
            icon.sprite = itemData.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false);
    }

    public ItemData_SO GetItem()
    {
        return Bag.items[Index].itemData;
    }
}

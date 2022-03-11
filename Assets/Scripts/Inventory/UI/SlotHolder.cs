using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    BAG,
    WEAPON,
    ARMOR,
    ACTION
}
public class SlotHolder : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;



    public void UseItem()
    {
        if (itemUI.GetItem() != null)
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                GameManager.Instance.playerState.ApplyHealth(itemUI.GetItem().useableData.Health);
                itemUI.Bag.items[itemUI.Index].amount -= 1;
                QuestManager.Instance.UpdateQuestProgress(itemUI.GetItem().itemName, -1);
            }
        UpdateItem();
    }

    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData;
                break;
            case SlotType.WEAPON:
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                if (itemUI.Bag.items[itemUI.Index].itemData != null)
                {
                    GameManager.Instance.playerState.ChengeWeapon(itemUI.Bag.items[itemUI.Index].itemData);
                }
                else
                {
                    GameManager.Instance.playerState.UnEquipWeapon();
                }
                break;
            case SlotType.ARMOR:
                itemUI.Bag = InventoryManager.Instance.equipmentData;

                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.Instance.actionData;
                break;
        }
        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData, item.amount);
    }
    #region ½Ó¿Ú
    public void OnPointerClick(PointerEventData eventData)
    {
        //ÅÐ¶ÏË«»÷
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.toolTip.SetUpTooltip(itemUI.GetItem());
            InventoryManager.Instance.toolTip.gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);

    }
    #endregion
    private void OnDisable()
    {
        InventoryManager.Instance.toolTip.gameObject.SetActive(false);
    }
}

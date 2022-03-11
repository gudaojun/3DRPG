using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentSlotHolder;
    SlotHolder targetSlotHoloder;
    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentSlotHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //��¼��ǰ��ק���������
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = currentSlotHolder;
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        //�Ƿ�����UI��
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //��ק���Ƿ����⼸��UI����
            if (InventoryManager.Instance.CheckInBagUI(eventData.position) ||
                InventoryManager.Instance.CheckInActionUI(eventData.position) ||
                InventoryManager.Instance.CheckInEquipmentUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetSlotHoloder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetSlotHoloder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }
                //�ж�Holder�Ƿ���ԭ����holder
                if (targetSlotHoloder != InventoryManager.Instance.currentDrag.originalHolder)
                    switch (targetSlotHoloder.slotType)
                    {
                        case SlotType.BAG:
                            SwapItem();
                            break;
                        case SlotType.WEAPON:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                                SwapItem();
                            break;
                        //����
                        case SlotType.ARMOR:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                                SwapItem();
                            break;
                        case SlotType.ACTION:
                            if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable)
                                SwapItem();
                            break;
                        default:
                            break;
                    }
                currentSlotHolder.UpdateItem();
                targetSlotHoloder.UpdateItem();
            }
            transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
            RectTransform t = transform as RectTransform;
            t.offsetMax = Vector2.zero;
            t.offsetMin = Vector2.zero;
        }
    }

    /// <summary>
    /// ����������Ϣ
    /// </summary>
    private void SwapItem()
    {
        var targetItem = targetSlotHoloder.itemUI.Bag.items[targetSlotHoloder.itemUI.Index];
        var tempItem = currentSlotHolder.itemUI.Bag.items[currentSlotHolder.itemUI.Index];
        bool isSameItem = tempItem.itemData == targetItem.itemData;
        if (isSameItem && targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentSlotHolder.itemUI.Bag.items[currentSlotHolder.itemUI.Index] = targetItem;
            targetSlotHoloder.itemUI.Bag.items[targetSlotHoloder.itemUI.Index] = tempItem;
        }
    }
}

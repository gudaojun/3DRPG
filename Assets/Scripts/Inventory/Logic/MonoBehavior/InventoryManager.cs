using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }
    //TODO ������ģ�����ڱ�������
    [Header("Inventory Data")]
    public InventoryData_SO inventoryTemplate;
    public InventoryData_SO inventoryData;
    public InventoryData_SO actionTemplate;
    public InventoryData_SO actionData;
    public InventoryData_SO equipmentTemplate;
    public InventoryData_SO equipmentData;

    [Header("Containers")]
    public ContainerUI bagUI;
    public ContainerUI actionUI;
    public ContainerUI equipmentUI;

    [Header("DrawCanvas")]
    public GameObject dragCanvas;

    public DragData currentDrag;
    [Header("Bag UI")]
    public GameObject bagPanel;
    [Header("Character State UI")]
    public GameObject statePanel;

    [Header("State Health")]
    public Text healthText;

    [Header("State Attack")]
    public Text attackText;

    [Header("Item Tooltip")]
    public ItemTooltip toolTip;
    bool isOpen = false;

    protected override void Awake()
    {
        base.Awake();
        if (inventoryTemplate != null)
            inventoryData = Instantiate(inventoryTemplate);
        if (actionTemplate != null)
            actionData = Instantiate(actionTemplate);
        if (equipmentTemplate != null)
            equipmentData = Instantiate(equipmentTemplate);
    }
    public void Start()
    {
        LoadData();
        bagUI.RefreshUI();
        actionUI.RefreshUI();
        equipmentUI.RefreshUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
            statePanel.SetActive(isOpen);
        }
        UpdateStateText(GameManager.Instance.playerState.CurrentHealth,
                                      GameManager.Instance.playerState.attackData.minDamge,
                                      GameManager.Instance.playerState.attackData.maxDamge);
    }

    public void SaveData()
    {
        SaveManager.Instance.Save(inventoryData, inventoryData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(equipmentData, equipmentData.name);

    }
    public void LoadData()
    {
        SaveManager.Instance.Load(inventoryData, inventoryData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(equipmentData, equipmentData.name);

    }
    public void UpdateStateText(int health, int min, int max)
    {
        healthText.text = health.ToString();
        attackText.text = min + " ~ " + max;
    }

    #region �������λ�����Ƿ��Ƕ�Ӧ��UI
    public bool CheckInBagUI(Vector3 pos)
    {
        for (int i = 0; i < bagUI.slotHolders.Count; i++)
        {
            RectTransform t = bagUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, pos))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInActionUI(Vector3 pos)
    {
        for (int i = 0; i < actionUI.slotHolders.Count; i++)
        {
            RectTransform t = actionUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, pos))
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckInEquipmentUI(Vector3 pos)
    {
        for (int i = 0; i < equipmentUI.slotHolders.Count; i++)
        {
            RectTransform t = equipmentUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, pos))
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region ���������Ʒ
    public void CheckQuestItemInBag(string questItemName)
    {
        foreach (var item in inventoryData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName, item.amount);
            }
        }
        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                    QuestManager.Instance.UpdateQuestProgress(item.itemData.itemName, item.itemData.itemAmount);
            }
        }
    }

    #endregion

    public InventoryItem QuestItemInBag(ItemData_SO questItem)
    {
        return inventoryData.items.Find(i => i.itemData == questItem);
    }

    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.itemData == questItem);
    }
}

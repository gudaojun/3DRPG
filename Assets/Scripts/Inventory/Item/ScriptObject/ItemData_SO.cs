using UnityEngine;

public enum ItemType
{
    Useable,//可使用的
    Weapon,
    Armor//盔甲装备
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    //类型
    public Sprite itemIcon;
    //名字
    public string itemName;
    //个数
    public int itemAmount;
    //详情
    [TextArea]
    public string description = "";
    //可堆叠的
    public bool stackable;

    [Header("Useable Item")]
    public UsableItemData_SO useableData;

    //实际生成的武器
    [Header("武器")]
    public GameObject prefab;

    public AttackData_SO attackData;
    [Header("动画控制器")]
    public AnimatorOverrideController weaponAnimator;
}

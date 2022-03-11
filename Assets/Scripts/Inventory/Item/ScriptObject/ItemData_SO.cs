using UnityEngine;

public enum ItemType
{
    Useable,//��ʹ�õ�
    Weapon,
    Armor//����װ��
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    //����
    public Sprite itemIcon;
    //����
    public string itemName;
    //����
    public int itemAmount;
    //����
    [TextArea]
    public string description = "";
    //�ɶѵ���
    public bool stackable;

    [Header("Useable Item")]
    public UsableItemData_SO useableData;

    //ʵ�����ɵ�����
    [Header("����")]
    public GameObject prefab;

    public AttackData_SO attackData;
    [Header("����������")]
    public AnimatorOverrideController weaponAnimator;
}

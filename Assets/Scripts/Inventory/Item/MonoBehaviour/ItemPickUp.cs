using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO iteamData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO:拾取物体
            //装备物体
            //添加到数据库
            InventoryManager.Instance.inventoryData.AddItem(iteamData, iteamData.itemAmount);
            InventoryManager.Instance.bagUI.RefreshUI();
            if (iteamData.itemType == ItemType.Weapon)
            {

                GameManager.Instance.playerState.EquidWeapon(iteamData);
            }
            QuestManager.Instance.UpdateQuestProgress(iteamData.itemName, iteamData.itemAmount);
            Destroy(gameObject);

        }
    }
}

using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO iteamData;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO:ʰȡ����
            //װ������
            //��ӵ����ݿ�
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

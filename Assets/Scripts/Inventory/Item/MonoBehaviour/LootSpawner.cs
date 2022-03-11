using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject item;
        [Range(0, 1)]
        public float weight;
    }
    public LootItem[] lootItems;
    /// <summary>
    /// 生成掉落物品
    /// </summary>
    public void SpawnLoot()
    {
        float currentNum = Random.value;

        for (int i = 0; i < lootItems.Length; i++)
        {
            if (currentNum <= lootItems[i].weight)
            {
                GameObject obj = Instantiate(lootItems[i].item);
                obj.transform.position = transform.position + Vector3.up * 2;
                obj.name = "Loot";
            }
        }
    }
}

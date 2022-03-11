using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "New QuestData_SO", menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;

    }
    //任务名
    public string questName;
    //描述
    public string description;
    //是否开始
    public bool isStarted;
    public bool isComplete;
    public bool isFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isComplete = finishRequires.Count() == questRequires.Count;
        if (isComplete)
        {
            //  isComplete = true;
            Debug.Log("任务完成");
        }
    }
    //当前任务中需要收集 消灭的目标名字列表
    public List<string> RequireTargetName()
    {
        List<string> targetNameList = new List<string>();
        foreach (var require in questRequires)
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }

    /// <summary>
    /// 给与任务奖励
    /// </summary>
    public void GiveRewards()
    {
        foreach (var reward in rewards)
        {
            if (reward.amount < 0)
            {
                int requireCount = Mathf.Abs(reward.amount);
                if (InventoryManager.Instance.QuestItemInBag(reward.itemData).amount <= requireCount)
                {
                    requireCount -= InventoryManager.Instance.QuestItemInBag(reward.itemData).amount;
                    InventoryManager.Instance.QuestItemInBag(reward.itemData).amount = 0;

                    if (InventoryManager.Instance.QuestItemInAction(reward.itemData) != null)
                    {
                        InventoryManager.Instance.QuestItemInAction(reward.itemData).amount -= requireCount;
                    }
                }
                else
                {
                    InventoryManager.Instance.QuestItemInBag(reward.itemData).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.Instance.inventoryData.AddItem(reward.itemData, reward.amount);
            }
            InventoryManager.Instance.bagUI.RefreshUI();
            InventoryManager.Instance.actionUI.RefreshUI();
        }
    }
}

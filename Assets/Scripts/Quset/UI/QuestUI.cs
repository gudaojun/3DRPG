using UnityEngine;
using UnityEngine.UI;
public class QuestUI : Singleton<QuestUI>
{
    [Header("Elment")]
    public GameObject questPanel;
    public ItemTooltip toolTip;
    bool isOpen;

    [Header("Quest Name")]
    public RectTransform questListTransform;
    public QuestButtonName questNameButton;

    [Header("Text Content")]
    public Text questContentText;

    [Header("Requirement")]
    public RectTransform requireTransform;
    public QuestRequirment requirement;

    //½±Àø
    [Header("Reward")]
    public RectTransform rewardTransfrom;
    public ItemUI rewardUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;
            questPanel.SetActive(isOpen);
            questContentText.text = "";
            SetUpQuestList();
            if (!isOpen)
            {
                toolTip.gameObject.SetActive(isOpen);
            }
        }
    }

    private void SetUpQuestList()
    {
        foreach (Transform item in questListTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in rewardTransfrom)
        {
            Destroy(item.gameObject);
        }
        foreach (var task in QuestManager.Instance.tasks)
        {
            var newTask = Instantiate(questNameButton, questListTransform);
            newTask.SetupButton(task.questData);
            newTask.questContentText = questContentText;
        }
    }
    public void SetUpRequireList(QuestData_SO questData)
    {
        foreach (Transform item in requireTransform)
        {
            Destroy(item.gameObject);
        }
        foreach (var require in questData.questRequires)
        {
            var q = Instantiate(requirement, requireTransform);
            if (questData.isFinished)
            {
                q.SetUpRequirement(requirement.name, true);
            }
            else
            {
                q.SetUpRequirement(require.name, require.requireAmount, require.currentAmount);
            }
        }
    }

    public void SetupRewardItem(ItemData_SO itemdata, int amount)
    {
        var item = Instantiate(rewardUI, rewardTransfrom);
        item.SetupItemUI(itemdata, amount);
    }
}

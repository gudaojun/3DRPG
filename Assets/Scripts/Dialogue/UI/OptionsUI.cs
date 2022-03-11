using UnityEngine;
using UnityEngine.UI;
public class OptionsUI : MonoBehaviour
{
    public Text optionText;
    private Button thisButton;
    private DialoguePiece currentPiece;
    private string nextPiceID;
    private bool takeQuest;
    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnIptionClicked);
    }
    public void UpdateOption(DialoguePiece piece, DialogueOption option)
    {
        currentPiece = piece;
        optionText.text = option.text;
        nextPiceID = option.targetID;
        takeQuest = option.takeQuest;
    }
    /// <summary>
    /// 点击对话选项按钮
    /// </summary>
    public void OnIptionClicked()
    {
        if (currentPiece.questData != null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.questData)
            };

            //是否接受任务
            if (takeQuest)
            {
                //添加到QuestManager的任务列表
                //判断列表里有这个任务吗
                if (QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //是否完成给与奖励
                    if (QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                    {
                        //判断背包与action栏有没有任务物品
                        newTask.questData.GiveRewards();
                        //设置任务完成
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    //没任务的话添加到任务列表
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;

                    foreach (var requireItem in newTask.questData.RequireTargetName())
                    {
                        InventoryManager.Instance.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }

        if (nextPiceID == "")
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.Instance.UpdateMainDiaologue(DialogueUI.Instance.currentData.dialogueIndex[nextPiceID]);
        }
    }
}

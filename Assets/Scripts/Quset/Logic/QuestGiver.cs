using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    DialogueController controller;
    QuestData_SO currentQuest;
    public DialogueData_so startDialogue;
    public DialogueData_so progressDialogue;
    public DialogueData_so completeDialogue;
    public DialogueData_so finishDialogue;

    #region 获得任务状态
    public bool IsStarted
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;
            }
            else return false;
        }
    }
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;
            }
            else return false;
        }
    }
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.HaveQuest(currentQuest))
            {
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;
            }
            else return false;
        }
    }
    #endregion

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }
    private void Start()
    {
        controller.currentData = startDialogue;
        currentQuest = controller.currentData.GetQuest();
    }
    private void Update()
    {
        if (IsStarted)
        {
            if (IsComplete)
            {
                controller.currentData = completeDialogue;
            }
            else
            {
                controller.currentData = progressDialogue;
            }
        }
        if (IsFinished)
        {
            controller.currentData = finishDialogue;
        }
    }
}

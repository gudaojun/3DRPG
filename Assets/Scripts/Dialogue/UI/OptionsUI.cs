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
    /// ����Ի�ѡ�ť
    /// </summary>
    public void OnIptionClicked()
    {
        if (currentPiece.questData != null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.questData)
            };

            //�Ƿ��������
            if (takeQuest)
            {
                //��ӵ�QuestManager�������б�
                //�ж��б��������������
                if (QuestManager.Instance.HaveQuest(newTask.questData))
                {
                    //�Ƿ���ɸ��뽱��
                    if (QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                    {
                        //�жϱ�����action����û��������Ʒ
                        newTask.questData.GiveRewards();
                        //�����������
                        QuestManager.Instance.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    //û����Ļ���ӵ������б�
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

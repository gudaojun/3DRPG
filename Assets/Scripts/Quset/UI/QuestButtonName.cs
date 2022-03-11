using UnityEngine;
using UnityEngine.UI;
public class QuestButtonName : MonoBehaviour
{
    public Text questNameText;

    public QuestData_SO currentData;

    public Text questContentText;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }
    private void UpdateQuestContent()
    {
        questContentText.text = currentData.description;
        QuestUI.Instance.SetUpRequireList(currentData);
        foreach (Transform item in QuestUI.Instance.rewardTransfrom)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in currentData.rewards)
        {
            QuestUI.Instance.SetupRewardItem(item.itemData, item.amount);
        }
    }
    public void SetupButton(QuestData_SO data)
    {
        currentData = data;
        if (data.isComplete)
        {
            questNameText.text = data.questName + ("ÕÍ≥…");

            //TODO:¡Ÿ ±≤‚ ‘
            data.isFinished = true;

        }
        else
        {
            questNameText.text = data.questName;
        }
    }
}

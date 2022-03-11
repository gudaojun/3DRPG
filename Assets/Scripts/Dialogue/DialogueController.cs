using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_so currentData;
    bool canTalk = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData)
        {
            canTalk = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueUI.Instance.dialoguePanel.gameObject.SetActive(false);
            canTalk = false;
        }
    }

    private void Update()
    {
        if (canTalk && Input.GetMouseButtonDown(1))
        {
            OpenDialogue();
        }
    }

    private void OpenDialogue()
    {
        DialogueUI.Instance.UpdateDiaologueData(currentData);
        //打卡UI面板加载对话
        DialogueUI.Instance.UpdateMainDiaologue(currentData.dialoguePieces[0]);
    }
}

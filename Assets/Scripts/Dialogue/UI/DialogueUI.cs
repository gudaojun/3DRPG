using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]

    public Image icon;
    public Text mainText;
    public Button nextButton;
    public GameObject dialoguePanel;
    public GameObject optionsPanel;
    public OptionsUI optionPrefab;
    [Header("Data")]
    public DialogueData_so currentData;

    int currentIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    private void ContinueDialogue()
    {
        if (currentData.dialoguePieces.Count > currentIndex)
            UpdateMainDiaologue(currentData.dialoguePieces[currentIndex]);
        else
            dialoguePanel.gameObject.SetActive(false);

    }

    public void UpdateDiaologueData(DialogueData_so data)
    {
        currentData = data;
        currentIndex = 0;
    }
    public void UpdateMainDiaologue(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);
        currentIndex++;
        if (piece.image != null)
        {
            icon.enabled = true;
            icon.sprite = piece.image;
        }
        else icon.enabled = false;
        mainText.text = "";
        mainText.DOText(piece.text, 1);

        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.interactable = true;
            nextButton.gameObject.SetActive(true);
            nextButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            nextButton.interactable = false;
            nextButton.transform.GetChild(0).gameObject.SetActive(false);
        }
        //加载生成选项Option
        CreatOptions(piece);
    }

    private void CreatOptions(DialoguePiece piece)
    {
        if (optionsPanel.transform.childCount > 0)
        {
            for (int i = 0; i < optionsPanel.transform.childCount; i++)
            {
                Destroy(optionsPanel.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < piece.options.Count; i++)
        {
            var option = Instantiate(optionPrefab, optionsPanel.transform);
            option.UpdateOption(piece, piece.options[i]);
        }
    }
}

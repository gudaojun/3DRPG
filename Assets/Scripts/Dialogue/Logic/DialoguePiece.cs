using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePiece
{

    public string ID;
    public Sprite image;
    [TextArea]
    public string text;
    public QuestData_SO questData;
    public List<DialogueOption> options = new List<DialogueOption>();

    [HideInInspector]
    //用于editor 能否折叠的变量
    public bool canFoldOut = false;
}

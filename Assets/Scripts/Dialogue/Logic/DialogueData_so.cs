using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueData", menuName = "Dialogue/Dialogue Data")]
public class DialogueData_so : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialogueIndex = new Dictionary<string, DialoguePiece>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        dialogueIndex.Clear();
        foreach (var piece in dialoguePieces)
        {
            if (!dialogueIndex.ContainsKey(piece.ID))
            {
                dialogueIndex.Add(piece.ID, piece);
            }
        }
    }
#endif

    public QuestData_SO GetQuest()
    {
        QuestData_SO currentQuest = null;
        foreach (var piece in dialoguePieces)
        {
            if (piece.questData)
            {
                currentQuest = piece.questData;
            }
        }
        return currentQuest;
    }
}

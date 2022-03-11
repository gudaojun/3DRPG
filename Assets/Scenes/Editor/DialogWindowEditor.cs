using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using System;
using System.IO;
[CustomEditor(typeof(DialogueData_so))]
public class DialogueCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Window"))
        {
            DialogWindowEditor.InitWindow((DialogueData_so)target);
        }
    }
}
public class DialogWindowEditor : EditorWindow
{
    DialogueData_so currentData;

    ReorderableList piecesList = null;
    Vector2 scrollPos = Vector2.zero;
    Dictionary<string, ReorderableList> optionListDict = new Dictionary<string, ReorderableList>();
    [MenuItem("Dialogue/Window")]
    public static void Init()
    {
        DialogWindowEditor editorWindow = GetWindow<DialogWindowEditor>("DialogWindow");
        //场景改变自动重新绘制窗口
        editorWindow.autoRepaintOnSceneChange = true;
    }
    public static void InitWindow(DialogueData_so data)
    {
        DialogWindowEditor editorWindow = GetWindow<DialogWindowEditor>("DialogWindow");
        editorWindow.currentData = data;
    }

    /// <summary>
    /// 双击打开窗口
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    [OnOpenAssetAttribute]
    public static bool ClickAsset(int instanceID, int line)
    {
        DialogueData_so data = EditorUtility.InstanceIDToObject(instanceID) as DialogueData_so;
        if (data != null)
        {
            InitWindow(data);
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        if (currentData != null)
        {
            EditorGUILayout.LabelField(currentData.name, EditorStyles.boldLabel);
            GUILayout.Space(10);

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            if (piecesList == null)
            {
                SetUpReorderableList();
            }
            //绘制Reorederab  没这个不会绘制
            piecesList.DoLayoutList();
            GUILayout.EndScrollView();
        }
        else
        {
            //没有选中DialogueData_so
            GUILayout.Label("NO DATA SELETED", EditorStyles.boldLabel);
            if(GUILayout.Button("创建新的对话数据"))
            {
                string path = "Assets/GameData/Dialogue Data/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //在目录下创建新资源并选中
                DialogueData_so newData=ScriptableObject.CreateInstance<DialogueData_so>();
                AssetDatabase.CreateAsset(newData, path+"New Dialogue.asset");
                currentData = newData;
            }
        }
    }

    private void SetUpReorderableList()
    {
        piecesList = new ReorderableList(currentData.dialoguePieces, typeof(DialoguePiece), true, true, true, true);
        piecesList.drawHeaderCallback += OnDrawPiecesHeader; //标题
        piecesList.drawElementCallback += OnPiecesDrawElement;//元素
        piecesList.elementHeightCallback += OnHeightChange;//元素高度
    }

    /// <summary>
    /// 设置一栏piece的高度
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private float OnHeightChange(int index)
    {
        return GetPieceHeight(currentData.dialoguePieces[index]);
    }
    private float GetPieceHeight(DialoguePiece piece)
    {
        var height = EditorGUIUtility.singleLineHeight;
        var canFoldout = piece.canFoldOut;
        if (canFoldout)
        {
            height += EditorGUIUtility.singleLineHeight * 9;
            var options = piece.options;
            if (options.Count > 1)
            {
                height += EditorGUIUtility.singleLineHeight * options.Count;
            }

        }
        return height;
    }

    private void OnPiecesDrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        //标记为数据 可以保存更改撤销
        EditorUtility.SetDirty(currentData);

        GUIStyle style = new GUIStyle("TextField");
        if (index < currentData.dialoguePieces.Count)
        {
            //ID
            var currentPieces = currentData.dialoguePieces[index];
            var tempRect = rect;
            tempRect.height = EditorGUIUtility.singleLineHeight;
            //折叠
            currentPieces.canFoldOut = EditorGUI.Foldout(tempRect, currentPieces.canFoldOut, currentPieces.ID);
            if (currentPieces.canFoldOut)
            {
                tempRect.width = 25;
                tempRect.y += tempRect.height;
                EditorGUI.LabelField(tempRect, "ID");
                tempRect.x += tempRect.width;
                tempRect.width = 100;
                currentPieces.ID = EditorGUI.TextField(tempRect, currentPieces.ID);

                //选择任务数据
                tempRect.x += tempRect.width + 20;
                EditorGUI.LabelField(tempRect, "任务数据");
                tempRect.x += 60;
                tempRect.width = 150;
                currentPieces.questData = (QuestData_SO)EditorGUI.ObjectField(tempRect, currentPieces.questData, typeof(QuestData_SO), false);

                //选择图片并从起一行
                tempRect.y += EditorGUIUtility.singleLineHeight + 10;
                tempRect.x = rect.x - 10;
                tempRect.height = 60;
                tempRect.width = tempRect.height;
                currentPieces.image = (Sprite)EditorGUI.ObjectField(tempRect, currentPieces.image, typeof(Sprite), false);

                //文本框 输入的对话
                tempRect.x += tempRect.width + 10;
                tempRect.width = rect.width - tempRect.width;
                //文本自动换行
                style.wordWrap = true;
                currentPieces.text = EditorGUI.TextField(tempRect, currentPieces.text, style);

                //选项数据栏
                tempRect.x = rect.x;
                tempRect.y += tempRect.height + 10;
                tempRect.width = rect.width;
                string key = currentPieces.ID + currentPieces.text;
                if (key != string.Empty)
                {
                    if (!optionListDict.ContainsKey(key))
                    {
                        var optionList = new ReorderableList(currentPieces.options, typeof(DialogueOption), true, true, true, true);
                        //Options 的具体绘制
                        optionList.drawHeaderCallback = OnDrawHeader;
                        optionList.drawElementCallback = (optionRect, optionIndex, optionActive, optionFocused) =>
                        {
                            OnDrawOptionElement(currentPieces, optionRect, optionIndex, optionActive, optionFocused);
                        };
                        optionListDict[key] = optionList;
                    }
                    optionListDict[key].DoList(tempRect);
                }

            }
        }
    }

    private void OnDrawHeader(Rect rect)
    {
        GUI.Label(rect, "按钮内容");
        rect.x += rect.width * 0.5f + 10;
        GUI.Label(rect, "下一句前往");
        rect.x += rect.width * 0.3f + 10;
        GUI.Label(rect, "接受任务");
    }

    private void OnDrawOptionElement(DialoguePiece currentPieces, Rect optionRect, int optionIndex, bool optionActive, bool optionFocused)
    {
        var currentOption = currentPieces.options[optionIndex];
        var tempRect = optionRect;
        tempRect.width = optionRect.width * 0.5f;
        currentOption.text = EditorGUI.TextField(tempRect, currentOption.text);

        tempRect.x += tempRect.width + 10;
        tempRect.width = optionRect.width * 0.3f;
        currentOption.targetID = EditorGUI.TextField(tempRect, currentOption.targetID);

        tempRect.x += tempRect.width + 10;
        tempRect.width = optionRect.width * 0.2f;
        currentOption.takeQuest = EditorGUI.Toggle(tempRect, currentOption.takeQuest);

    }


    //标题栏
    private void OnDrawPiecesHeader(Rect rect)
    {
        GUI.Label(rect, "对话内容");
    }

    //每当选择发生更改的时候调用
    private void OnSelectionChange()
    {
        var newData = Selection.activeObject as DialogueData_so;
        if (newData != null)
        {
            currentData = newData;
            SetUpReorderableList();
        }
        else
        {
            currentData = null;
            piecesList = null;
        }
        Repaint();
    }
    private void OnDisable()
    {
        optionListDict.Clear();
    }
}

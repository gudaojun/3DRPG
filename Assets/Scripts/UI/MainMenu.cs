using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class MainMenu : MonoBehaviour
{
    Button newGame;
    Button continueGame;
    Button quitGame;

    PlayableDirector playableDirector;
    private void Awake()
    {
        Transform group = transform.GetChild(1);

        newGame=group.GetChild(0).GetComponent<Button>();
        continueGame= group.GetChild(1).GetComponent<Button>();
        quitGame= group.GetChild(2).GetComponent<Button>();

        newGame.onClick.AddListener(PlayTimeLine);
        continueGame.onClick.AddListener(ContinueGame);
        quitGame.onClick.AddListener(QuitGame);

        playableDirector = FindObjectOfType<PlayableDirector>();
        playableDirector.stopped += NewGame;
    }

    private void PlayTimeLine()
    {
        playableDirector.Play();
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void ContinueGame()
    {
        //跳转场景 加载数据
        SceneMgr.Instance.LoadGame();
    }

    private void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        //加载场景
        SceneMgr.Instance.LoadFristLevel();
    }
}

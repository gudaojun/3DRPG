using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class SceneMgr : Singleton<SceneMgr>, IEndGameObserver
{
    public GameObject playerPrefab;
    GameObject player;
    NavMeshAgent meshAgent;

    public FadeCanvas fadeCanvasPrefab;
    private bool isFinshied ;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        GameManager.Instance.AddObserver(this);
        isFinshied = true;
    }

    public void TranstionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                //StartCoroutine(LoadSceneAsync("NextScene", transitionPoint.destinationTag, () =>
                //{
                // //   Player = GameManager.Instance.playerState.gameObject;
                //    Instantiate(playerPrefab).transform.SetPositionAndRotation(
                //        GetDestination(transitionPoint.destinationTag).transform.position,
                //        GetDestination(transitionPoint.destinationTag).transform.rotation);
                //}));
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;
            default:
                break;
        }
    }
    IEnumerator Transition(string scenName, TransitionDestion.DestinationTag destinationTag)
    {
        //TODO:保存数据
        SaveManager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();
        QuestManager.Instance.SaveQuestManager();
        if (SceneManager.GetActiveScene().name != scenName)
        {
            var fade = Instantiate(fadeCanvasPrefab);
            yield return StartCoroutine(fade.FadeIn(1.5f));
            yield return SceneManager.LoadSceneAsync(scenName);
            yield return Instantiate(playerPrefab, GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            yield return StartCoroutine(fade.FadeOut(1.5f));
            SaveManager.Instance.LoadPlayerData();
            yield break;
        }
        else
        {
            player = GameManager.Instance.playerState.gameObject;
            meshAgent = player.GetComponent<NavMeshAgent>();
            meshAgent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            meshAgent.enabled = true;
        }

        yield return null;
    }


    IEnumerator LoadSceneAsync(string scenName, TransitionDestion.DestinationTag destinationTag, Action action = null)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenName);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            //TODO :进度条
            yield return new WaitForEndOfFrame();
        }
        operation.allowSceneActivation = true;
        action?.Invoke();
        yield return null;
    }

    private TransitionDestion GetDestination(TransitionDestion.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestion>();
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null;
    }

    public void LoadFristLevel()
    {
        StartCoroutine(LoadLevel("Game"));
    }

    IEnumerator LoadLevel(string scene)
    {
        FadeCanvas fade = Instantiate(fadeCanvasPrefab);
        if (scene != null)
        {
            yield return StartCoroutine(fade.FadeIn(1.5f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return Instantiate(playerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);
            //保存数据
            SaveManager.Instance.SavePlayerData();
            yield return StartCoroutine(fade.FadeOut(1.5f));
        }
        yield break;
    }
    public void LoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }
    public void LoadMain()
    {
        StartCoroutine(LoadMainScene());
    }
    IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(3);
        FadeCanvas fade = Instantiate(fadeCanvasPrefab);
        yield return StartCoroutine(fade.FadeIn(1.5f));
        yield return SceneManager.LoadSceneAsync("Main");
        yield return StartCoroutine(fade.FadeOut(1.5f));
        yield break;
    }

    public void EndNotify()
    {
        if (isFinshied)
        {
            isFinshied = false;
            StartCoroutine(LoadMainScene());
        }
    }
}

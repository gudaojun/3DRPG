using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveManager : Singleton<SaveManager>
{
    string sceneName;
    public string SceneName
    {
        get { return PlayerPrefs.GetString(sceneName); }
    }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void SavePlayerData()
    {
        Save(GameManager.Instance.playerState.characterData, GameManager.Instance.playerState.characterData.name);
    }
    public void LoadPlayerData()
    {
        Load(GameManager.Instance.playerState.characterData, GameManager.Instance.playerState.characterData.name);
    }

    public void Save(object data, string name)
    {
        var json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(name, json);
        PlayerPrefs.SetString(sceneName,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(object data, string name)
    {
        if (PlayerPrefs.HasKey(name))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(name), data);

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneMgr.Instance.LoadMain();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
            QuestManager.Instance.SaveQuestManager();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadPlayerData();
        }
    }
}

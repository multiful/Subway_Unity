using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private UserData userData;
    private ScriptData scriptData;

    private List<IScriptDataPersistence> scriptDataPersistenceObjects;
    private List<IUserDataPersistence> userDataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    private Coroutine autoSaveCoroutine;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);


        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)  
    {
        this.userDataPersistenceObjects = FindAllDataPersistenceObjects();
        this.scriptDataPersistenceObjects = FindAllScriptDataPersistenceObjects();
        LoadGame();

        // start up the auto saving coroutine
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        // update the profile to use for saving and loading
        this.selectedProfileId = newProfileId;
        // load the game, which will use that profile, updating our game data accordingly
        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        // delete the data for this profile id
        dataHandler.Delete(profileId);
        // initialize the selected profile id
        InitializeSelectedProfileId();
        // reload the game so that our data matches the newly selected profile id
        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    public void NewGame()
    {
        this.userData = new UserData();
        this.scriptData = new ScriptData();
    }

    public void LoadGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // load any saved data from a file using the data handler
        this.userData = dataHandler.Load(selectedProfileId);

        // start a new game if the data is null and we're configured to initialize data for debugging purposes
        if (this.userData == null && initializeDataIfNull)
        {
            NewGame();
        }

        // if no data can be loaded, don't continue
        if (this.userData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        // push the loaded data to all other scripts that need it
        // 로드된 데이터를 필요한 다른 모든 스크립트로 푸시한다.
        foreach (IUserDataPersistence userDataPersistenceObj in userDataPersistenceObjects)
        {
            userDataPersistenceObj.LoadData(userData);
        }
        foreach (IScriptDataPersistence scriptDataPersistenceObj in scriptDataPersistenceObjects)
        {
            scriptDataPersistenceObj.LoadData(scriptData);
        }
    }

    public void SaveGame()
    {
        // return right away if data persistence is disabled
        if (disableDataPersistence)
        {
            return;
        }

        // if we don't have any data to save, log a warning here
        if (this.userData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        // pass the data to other scripts so they can update it
        // 데이터를 다른 스크립트에 전달하여 업데이트 하도록 한다.
        foreach (IUserDataPersistence userDataPersistenceObj in userDataPersistenceObjects)
        {
            userDataPersistenceObj.SaveData(userData);
        }
        foreach (IScriptDataPersistence scriptPersistenceObj in scriptDataPersistenceObjects)
        {
            scriptPersistenceObj.SaveData(scriptData);
        }

        // timestamp the data so we know when it was last saved
        userData.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        // 데이터 핸들러를 사용해서 파일에 저장
        dataHandler.Save(userData, selectedProfileId);

        Debug.Log($"DataPersistenceManager 저장완료");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IUserDataPersistence> FindAllDataPersistenceObjects()
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IUserDataPersistence> userDataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IUserDataPersistence>();

        return new List<IUserDataPersistence>(userDataPersistenceObjects);
    }

    private List<IScriptDataPersistence> FindAllScriptDataPersistenceObjects()
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IScriptDataPersistence> scriptDataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IScriptDataPersistence>();

        return new List<IScriptDataPersistence>(scriptDataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return userData != null;
    }

    public Dictionary<string, UserData> GetAllProfilesGameData() //clear 3-904
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved Game");
        }
    }
}

using Naninovel;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour, IUserDataPersistence
{
    public float score_GetOnSub = 0;
    public float timeLimit = 60f;
    public TMP_Text timerText;
    public TMP_Text targetText;
    public TMP_Text alertText;
    public TMP_Text rewardText; // 보상을 표시할 텍스트 추가
    public GameObject gameOverText;
    public GameObject gameClearText;
    public GameObject player;
    public ObstacleManager obstacleManager;
    [SerializeField] private Button deleteButton;

    private float currentTime;
    private int remainingTargets;
    private int difficultyLevel = 1; // 난이도 초기값
    private bool isGameOver = false;
    private int gameID = 1; // 현재 게임의 ID

    [SerializeField]
    private UserData userData;
    

    void Start()
    {
        gameOverText.SetActive(false);
        gameClearText.SetActive(false);
        alertText.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false); // 보상 텍스트 숨김
        currentTime = timeLimit;


        // DataPersistenceManager.instance.DeleteProfileData(); // 데이터 삭제.

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(OnNewGameButtonClicked);
        }

        // 데이터를 로드하고 userData 초기화 // 필요없음
        /*DataPersistenceManager.instance.LoadGame();
        if (userData == null)
        {
            Debug.LogError("UserData is null after loading. Creating new UserData instance.");
            userData = new UserData();
        }*/
        SetDifficultyLevel();
        UpdateUI();
        obstacleManager.GenerateInitialObstacles();
    }

    void Update()
    {
        if (!isGameOver)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                EndGame();
            }
            UpdateUI();
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void HitObstacle()
    {
        if (!isGameOver)
        {
            currentTime -= 5f;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                EndGame();
            }
            ShowAlert("Time -5");
            UpdateUI();
        }
    }

    public void PassLine()
    {
        if (!isGameOver)
        {
            if (remainingTargets > 0)
            {
                remainingTargets--;
                if (remainingTargets <= 0)
                {
                    ClearGame();
                }
            }
            UpdateUI();
        }
    }

    public async void Transfer()
    {
        
        // 2. Switch cameras.
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;

        // 3. Load and play specified script (if assigned).
        await GameManager.Nani.ScriptPlayer.PreloadAndPlayAsync("빠르게환승", label: "실패1");

        // 4. Enable Naninovel input.
        GameManager.Nani.InputManager.ProcessInput = true;

        gameObject.SetActive(false);
    }
    void UpdateUI()
    {
        timerText.text = "" + Mathf.CeilToInt(currentTime).ToString();
        targetText.text = "" + remainingTargets.ToString();
    }

    void EndGame()
    {
        isGameOver = true;
        Debug.Log("Game Over");
        gameOverText.SetActive(true);
        player.GetComponent<PlayerController1>().enabled = false;
        obstacleManager.enabled = false;

        Transfer();

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }

        Time.timeScale = 0;
    }

    void ClearGame()
    {
        Debug.Log("Game Cleared");
        gameClearText.SetActive(true);
        player.GetComponent<PlayerController1>().enabled = false;
        obstacleManager.enabled = false;
        Time.timeScale = 0;

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obstacle);
        }

        // 클리어 처리
        CalculateReward();
        SaveGameProgress();

        // 2. Switch cameras.
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;

        // Naninovel 대화 스크립트 호출 (성공 시 성공1 라벨로 이동)
        var runner = Engine.GetService<IScriptPlayer>();
        runner.PreloadAndPlayAsync("빠르게환승", label: "성공1").Forget();

        // 4. Enable Naninovel input.
        GameManager.Nani.InputManager.ProcessInput = true;
    }

    void SetDifficultyLevel()
    {
        // userData가 null인지 확인
        if (userData == null)
        {
            Debug.LogError("UserData is null. Cannot set difficulty level.");
            return;
        }

        // gameAndDifficultyCleared가 null인지 확인하고 초기화
        if (userData.gameAndDifficultyCleared == null)
        {
            userData.gameAndDifficultyCleared = new SerializableDictionary<int, SerializableDictionary<int, bool>>();
        }

        // gameID에 해당하는 난이도 데이터가 있는지 확인하고 초기화
        if (!userData.gameAndDifficultyCleared.ContainsKey(gameID))
        {
            userData.gameAndDifficultyCleared[gameID] = new SerializableDictionary<int, bool>();
        }
        

        // 난이도 설정
        for (int level = 1; level <= 4; level++)
        {
            if (!userData.gameAndDifficultyCleared[gameID].ContainsKey(level) || !userData.gameAndDifficultyCleared[gameID][level])
            {
                difficultyLevel = level;
                remainingTargets = 30 + (difficultyLevel - 1) * 5;
                return;
            }
        }

        // 기본 난이도 설정
        difficultyLevel = 1;
        remainingTargets = 30;
    }


    void CalculateReward()
    {
        int reward = Mathf.CeilToInt(currentTime) * 500 * difficultyLevel;
        rewardText.text = $" +  {reward}";
        rewardText.gameObject.SetActive(true);
        userData.money += reward;

        SaveData(userData);
        Debug.Log($"UserData + reward = {userData.money} ");
    }

    void SaveGameProgress()
    {
        // 현재 난이도를 완료한 것으로 설정
        if (!userData.gameAndDifficultyCleared.ContainsKey(gameID))
        {
            userData.gameAndDifficultyCleared[gameID] = new SerializableDictionary<int, bool>();
        }

        userData.gameAndDifficultyCleared[gameID][difficultyLevel] = true;
        

        // DataPersistenceManager를 사용해 게임 데이터를 저장
        DataPersistenceManager.instance.SaveGame();
    }

    public void LoadData(UserData data)
    {
        
        userData.gameAndDifficultyCleared = data.gameAndDifficultyCleared; 
        userData.money = data.money;
        Debug.Log($"데이터가 로드 되었습니다. money = {data.money}");
    }

    public void SaveData(UserData data)
    {
        data.gameAndDifficultyCleared = userData.gameAndDifficultyCleared;
        data.money = userData.money;
        Debug.Log($"현재 유저의 돈: {userData.money}, difficulty = {difficultyLevel}");

    }

    public void ReduceTimeByFive()
    {
        currentTime -= 5f;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            EndGame();
        }
        ShowAlert("Time -5");
        UpdateUI();
    }

    private void ShowAlert(string message)
    {
        StartCoroutine(DisplayAlert(message));
    }

    private IEnumerator DisplayAlert(string message)
    {
        alertText.text = message;
        alertText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        alertText.gameObject.SetActive(false);
    }


    private void OnNewGameButtonClicked()
    {
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.NewGame();
            Debug.Log("New Game");
        }
        else
        {
            Debug.LogError("DataPersistentManager instance not found!");
        }
    }
    
}

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
    
    private UserData userData;

    void Start()
    {
        gameOverText.SetActive(false);
        gameClearText.SetActive(false);
        alertText.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false); // 보상 텍스트 숨김
        currentTime = timeLimit;

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(OnNewGameButtonClicked);
        }

        // 데이터를 로드하고 userData 초기화
        DataPersistenceManager.instance.LoadGame();
        if (userData == null)
        {
            Debug.LogError("UserData is null after loading. Creating new UserData instance.");
            userData = new UserData();
        }
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

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();
        targetText.text = "Targets: " + remainingTargets.ToString();
    }

    void EndGame()
    {
        isGameOver = true;
        Debug.Log("Game Over");
        gameOverText.SetActive(true);
        player.GetComponent<PlayerController1>().enabled = false;
        obstacleManager.enabled = false;

        // Naninovel 대화 스크립트 호출
        /*var runner = Engine.GetService<IScriptPlayer>();
        runner.PreloadAndPlayAsync("reward-dialogue");*/

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
        rewardText.text = $"보상: {reward}원";
        rewardText.gameObject.SetActive(true);
        userData.money += reward;
        SaveData(userData);
    }

    void SaveGameProgress()
    {
        userData.gameAndDifficultyCleared[gameID][difficultyLevel] = true;
        DataPersistenceManager.instance.SaveGame();


    }

    public void LoadData(UserData data)
    {
        // userData.money = data.money;
        userData = data;
    }

    public void SaveData(UserData data)
    {
        data.gameAndDifficultyCleared = userData.gameAndDifficultyCleared;
        data.money = userData.money;
        Debug.Log($"현재 유저의 돈: {userData.money}");

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

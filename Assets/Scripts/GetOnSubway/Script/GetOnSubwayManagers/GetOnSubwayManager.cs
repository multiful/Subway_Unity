using Naninovel;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetOnSubwayManager : MonoBehaviour
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
    private int difficultyLevel;
    private readonly int[] target = { 30, 35, 40, 45 };
    private bool isGameOver = false;



    void Start()
    {
        gameOverText.SetActive(false);
        gameClearText.SetActive(false);
        alertText.gameObject.SetActive(false);
        rewardText.gameObject.SetActive(false); // 보상 텍스트 숨김
        currentTime = timeLimit;


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

        GameManager.Nani.PlayNani("빠르게환승", "실패" + (difficultyLevel + 1).ToString());

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

        GameManager.Nani.PlayNani("빠르게환승", "성공" + (difficultyLevel + 1).ToString());
    }

    void SetDifficultyLevel()
    {
        difficultyLevel = GameManager.userData.CurrentGameLevel;
        remainingTargets = target[difficultyLevel];
    }


    void CalculateReward()
    {
        int reward = Mathf.CeilToInt(currentTime) * 500;
        rewardText.text = $" +  {reward}";
        rewardText.gameObject.SetActive(true);

        var varManager = Engine.GetService<ICustomVariableManager>();
        varManager.TrySetVariableValue("reward", reward);
        GameManager.userData.Money += reward;

        Debug.Log($"UserData + reward = {GameManager.userData.Money} ");
        GameManager.userData.IsGameClear[0, difficultyLevel] = true;
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

    
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
    public float timeLimit = 120f; // 게임의 시간 제한 (초 단위)
    public int targetCount = 4; // 목표 통과 수
    public TMP_Text timerText; // UI에 남은 시간을 표시할 텍스트
    public TMP_Text targetText; // UI에 남은 목표 통과 수를 표시할 텍스트

    private float currentTime; // 현재 시간
    private int remainingTargets; // 남은 목표 통과 수

    void Start()
    {
        currentTime = timeLimit;
        remainingTargets = targetCount;
        UpdateUI();
        InvokeRepeating("UpdateTimer", 1f, 1f); // 1초마다 UpdateTimer 함수를 호출하여 시간 업데이트
    }

    void UpdateTimer()
    {
        currentTime -= 1f;
        if (currentTime <= 0f)
        {
            EndGame();
        }
        UpdateUI();
    }

    public void HitObstacle()
    {
        currentTime -= 5f;
        if (currentTime <= 0f)
        {
            EndGame();
        }
        UpdateUI();
    }

    public void PassLine()
    {
        remainingTargets--;
        if (remainingTargets <= 0)
        {
            ClearGame();
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        timerText.text = "Time: " + Mathf.CeilToInt(currentTime).ToString();
        targetText.text = "Targets: " + remainingTargets.ToString();
    }

    void EndGame()
    {
        Debug.Log("Game Over");
        // 게임 오버 처리
    }

    void ClearGame()
    {
        Debug.Log("Game Cleared");
        // 클리어 처리
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public GameManager1 gameManager; // GameManager1 스크립트 참조
    public ObstacleManager obstacleManager; // ObstacleManager 스크립트 참조

    private int currentLane = 1; // 주인공의 현재 위치 (0: 왼쪽, 1: 중앙, 2: 오른쪽)
    public float laneWidth = 3f; // 장애물 간의 간격

    private void Update()
    {
        // 좌우 화살표 키 입력을 감지하여 주인공을 이동시킵니다.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    public void MoveLeft()
    {
        if (currentLane > 0)
        {
            Vector3 targetPosition = transform.position + Vector3.left * laneWidth;
            if (CanMoveToPosition(targetPosition))
            {
                currentLane--;
                transform.position = targetPosition;
                ProcessMovement();
            }
            else
            {
                gameManager.HitObstacle(); // 장애물에 부딪히면 시간만 감소
            }
        }
        else
        {
            // 가장 왼쪽에서 왼쪽으로 더 이동 시도 시
            gameManager.ReduceTimeByFive();
        }
    }

    public void MoveRight()
    {
        if (currentLane < 2)
        {
            Vector3 targetPosition = transform.position + Vector3.right * laneWidth;
            if (CanMoveToPosition(targetPosition))
            {
                currentLane++;
                transform.position = targetPosition;
                ProcessMovement();
            }
            else
            {
                gameManager.HitObstacle(); // 장애물에 부딪히면 시간만 감소
            }
        }
        else
        {
            // 가장 오른쪽에서 오른쪽으로 더 이동 시도 시
            gameManager.ReduceTimeByFive();
        }
    }

    private void ProcessMovement()
    {
        // 플레이어 이동 후 장애물 이동
        obstacleManager.MoveObstaclesDown();

        // 장애물 충돌 여부 확인
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        bool hitObstacle = false;
        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Obstacle"))
            {
                
                hitObstacle = true;
                Destroy(collider.gameObject); // 충돌한 장애물 제거
                break;
            }
        }

        if (!hitObstacle)
        {
            // Debug.Log("PassLine -> 목표수 -1");
            gameManager.PassLine();
        }
    }

    private bool CanMoveToPosition(Vector3 targetPosition)
    {
        int targetLane = (int)((targetPosition.x / laneWidth) + 1); // 목표 위치의 레인
        if (obstacleManager.IsLaneClear(targetLane))
        {
            return true; // 이동 가능
        }
        return false; // 이동 불가
    }
}

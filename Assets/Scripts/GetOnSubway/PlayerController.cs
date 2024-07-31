using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.5f; // 주인공의 이동 속도
    public float laneWidth = 1.5f; // 장애물 간의 간격

    private int currentLane = 1; // 주인공의 현재 위치 (0: 왼쪽, 1: 중앙, 2: 오른쪽)

    void Update()
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

    void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
            transform.position += Vector3.left * laneWidth;
        }
    }

    void MoveRight()
    {
        if (currentLane < 2)
        {
            currentLane++;
            transform.position += Vector3.right * laneWidth;
        }
    }
}

using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // 장애물의 이동 속도

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.down);
    }
}
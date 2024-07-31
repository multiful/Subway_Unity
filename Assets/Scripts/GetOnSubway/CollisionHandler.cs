using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameManager1 gameManager; // GameManager 스크립트 참조

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            gameManager.HitObstacle(); // 장애물과 충돌할 때 시간 감소
        }
    }
}
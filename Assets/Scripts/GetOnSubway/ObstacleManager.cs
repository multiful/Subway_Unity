using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public int numberOfLines = 3; // 장애물 줄(열)의 개수
    public GameObject obstaclePrefab; // 장애물 프리팹

    void Start()
    {
        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for (int i = 0; i < numberOfLines; i++)
        {
            List<int> emptySpaces = GetEmptySpaces();
            for (int j = 0; j < 3; j++)
            {
                // 장애물을 생성할 위치 결정
                Vector3 position = new Vector3(j, i, 0);
                if (emptySpaces.Contains(j))
                {
                    // 빈 칸이면 장애물을 생성하지 않음
                    continue;
                }
                Instantiate(obstaclePrefab, position, Quaternion.identity);
            }
        }
    }

    List<int> GetEmptySpaces()
    {
        List<int> emptySpaces = new List<int>();
        int emptySpaceCount = Random.Range(1, 4); // 최소 1개에서 최대 3개의 빈 칸
        for (int i = 0; i < emptySpaceCount; i++)
        {
            emptySpaces.Add(Random.Range(0, 3)); // 빈 칸의 위치를 랜덤하게 선택
        }
        return emptySpaces;
    }
}

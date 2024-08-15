using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // 장애물 프리팹 배열
    public GameObject player;
    public GameManager1 gameManager; // GameManager1 스크립트 참조
    public float obstacleGap = 5.0f; // 장애물 간의 간격
    private List<GameObject> obstacles = new List<GameObject>(); // 생성된 장애물들을 저장할 리스트
    private List<ObstacleData[]> grid = new List<ObstacleData[]>(); // 장애물 위치와 이미지를 저장할 그리드
    private int gridCols = 3; // 그리드의 열 수
    private int count = 0; // 행 생성을 위한 카운트 변수

    void Start()
    {
        // Set all obstacle prefabs to active
        foreach (var prefab in obstaclePrefabs)
        {
            prefab.SetActive(true);
        }
        GenerateInitialObstacles();
    }

    public void GenerateInitialObstacles()
    {
        // 초기 장애물 생성 (플레이어 머리 위로 3개의 행 생성)
        for (int i = 0; i < 3; i++)
        {
            GenerateObstacleRow(i);
        }
        UpdateObstaclePositions();
    }

    public void MoveObstaclesDown()
    {
        // 새로운 행 생성
        GenerateObstacleRow(grid.Count);

        // 모든 장애물의 위치 업데이트
        UpdateObstaclePositions();
    }

    private void GenerateObstacleRow(int row)
    {
        int emptySpace = GetNextEmptySpace(row); // 빈 공간 위치 결정
        int obstacleCount = Random.Range(1, 3); // 1 또는 2개의 장애물 생성

        ObstacleData[] newRow = new ObstacleData[gridCols];
        for (int i = 0; i < gridCols; i++)
        {
            newRow[i] = new ObstacleData { hasObstacle = false, prefabIndex = -1 }; // 먼저 모든 칸을 빈 공간으로 초기화
        }

        // 장애물 생성
        int generatedObstacles = 0;
        while (generatedObstacles < obstacleCount)
        {
            for (int i = 0; i < gridCols; i++)
            {
                if (generatedObstacles >= obstacleCount)
                    break;

                // 빈 공간이 아닌 곳에만 장애물 배치
                if (i != emptySpace)
                {
                    newRow[i].hasObstacle = true;
                    newRow[i].prefabIndex = Random.Range(0, obstaclePrefabs.Length);
                    generatedObstacles++;
                }
            }
        }

        if (count > 2)
        {
            grid.Insert(0, newRow); // 새로운 행을 그리드의 맨 앞에 추가
        }
        count++;

        RemoveOldRows();
    }

    private int GetNextEmptySpace(int row)
    {
        if (row % 2 == 0) // 짝수 행
        {
            int[] possibleCols = { 0, 2 }; // 1열 또는 3열 (index 0 또는 2)
            return possibleCols[Random.Range(0, possibleCols.Length)];
        }
        else // 홀수 행
        {
            return 1; // 2열 (index 1)
        }
    }

    private void UpdateObstaclePositions()
    {
       
        float startY = player.transform.position.y + 14.4f;
        

        // 기존 장애물 제거
        ClearExistingObstacles();

        Quaternion fixedRotation = Quaternion.Euler(0, 0, 0);

        // 고정된 크기 설정
        Vector3 fixedScale = new Vector3(0.6420706f, 0.37f,0);

        // 장애물 생성 및 위치 업데이트
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < gridCols; j++)
            {
                if (grid[i][j].hasObstacle)
                {
                    Vector3 position = new Vector3((j - 1) * obstacleGap, startY - i * obstacleGap, 0);

                    // i 값에 따라 z 축 위치를 변경하여 레이어 우선순위 설정
                    Vector3 adjustedPosition = new Vector3(position.x, position.y, -i * 0.1f);

                    GameObject obstacle = Instantiate(obstaclePrefabs[grid[i][j].prefabIndex], adjustedPosition, fixedRotation);
                    obstacle.transform.localScale = fixedScale;
                    obstacles.Add(obstacle);
                }
            }
        }
    }

    public bool IsLaneClear(int targetLane)
    {
        // 이동하려는 위치의 레인이 빈 공간인지 확인
        return !grid[2][targetLane].hasObstacle;
    }

    private void ClearExistingObstacles()
    {
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        obstacles.Clear();
    }

    public void RemoveObstacle(GameObject obstacle)
    {
        if (obstacles.Contains(obstacle))
        {
            obstacles.Remove(obstacle);
            Destroy(obstacle);
        }
    }

    private void RemoveOldRows()
    {
        // 오래된 행이 존재하고 그리드의 길이가 6을 초과할 경우 오래된 행 제거
        while (grid.Count > 6)
        {
            grid.RemoveAt(grid.Count - 1);
            grid.RemoveAt(grid.Count - 2);
        }
    }

    private class ObstacleData
    {
        public bool hasObstacle;
        public int prefabIndex;
    }
}

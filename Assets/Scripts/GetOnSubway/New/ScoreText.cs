using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour, IUserDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    // 게임별 난이도별 점수를 저장하는 딕셔너리
    public SerializableDictionary<int, SerializableDictionary<int, bool>> gameAndDifficultyCleared;

    // 현재 게임과 난이도를 저장하는 변수
    public int currentGame = 1;
    public int currentDifficulty = 1;

    public NowStoryName episod;
    private TextMeshProUGUI scoreText;

    public void LoadData(UserData data)
    {
        this.gameAndDifficultyCleared = data.gameAndDifficultyCleared;
    }

    public void SaveData(UserData data)
    {
        data.gameAndDifficultyCleared = this.gameAndDifficultyCleared;
    }

    public void LoadDataS(ScriptData data)
    {
        this.episod = data.save_ep;
    }

    public void SaveDataS(ScriptData data)
    {
        data.save_ep = this.episod;
    }

    private void Awake()
    {
        scoreText = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (gameAndDifficultyCleared.ContainsKey(currentGame) && gameAndDifficultyCleared[currentGame].ContainsKey(currentDifficulty))
        {
            scoreText.text = "" + gameAndDifficultyCleared[currentGame][currentDifficulty];
        }
        else
        {
            scoreText.text = "0";
        }
    }
}

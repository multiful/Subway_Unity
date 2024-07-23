using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UserData2
{
    public string _userName { get; set; }
    public NowStoryName _lastEpisod { get; set; }
    public int _lastLine { get; set; }
    public int _ticket { get; set; }
    public int _money { get; set; }
    public int _eyeDrops { get; set; }
    public List<Diary> _diaryList { get; set; }
    public List<int> _ending { get; set; }

    public Dictionary<int, ScriptData2> dataList = new Dictionary<int, ScriptData2>();
    
    // 게임별 난이도별 점수를 저장하는 딕셔너리
    public Dictionary<int, Dictionary<int, int>> scoreByGameAndDifficulty = new Dictionary<int, Dictionary<int, int>>();

    public UserData2()
    {
        _userName = string.Empty;
        _lastEpisod = new NowStoryName();
        _lastLine = 0;
        _ticket = 0;
        _money = 0;
        _eyeDrops = 0;
        _diaryList = new List<Diary>();
        _ending = new List<int>();
        dataList = new Dictionary<int, ScriptData2>();
        scoreByGameAndDifficulty = new Dictionary<int, Dictionary<int, int>>();
    }
}
[Serializable]
public class ScriptData2
{
    public NowStoryName _episod { get; set; }
    public int _line { get; set; }
    public int _likeability { get; set; }
}

public class SaveLoadManager : MonoBehaviour
{
    private static SaveLoadManager _Inst;
    public static SaveLoadManager Inst
    {
        get
        {
            if (_Inst == null)
            {
                return null;
            }
            return _Inst;
        }
    }

    // 현재 매니저에 장착된 유저 데이터
    private UserData2 _userData;
    public UserData2 userData
    {
        get
        {
            return _userData;
        }
    }

    private void Awake()
    {
        if (_Inst == null)
        {
            _Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadUserData();
    }

    string DataToJson(object data)
    {
        return JsonUtility.ToJson(data);
    }

    T JsonToData<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }

    public void SaveUserData()
    {
        Debug.Log("유저 데이터 저장");
        PlayerPrefs.SetString("GameData", DataToJson(_userData));
        PlayerPrefs.Save();
    }

    public void LoadUserData()
    {
        if (!PlayerPrefs.HasKey("GameData"))
        {
            Debug.Log("게임 데이터 없음");
            _userData = new UserData2();
        }
        else
        {
            Debug.Log("게임 데이터 존재");
            _userData = JsonToData<UserData2>(PlayerPrefs.GetString("GameData"));
            Debug.Log(_userData.dataList.Count + "개 스토리 저장되어 있음");
        }
    }

    public void SaveScriptData(int index)
    {
        Debug.Log("스크립트 데이터 저장");
        ScriptData2 data = new ScriptData2
        {
            _episod = GameManager.Data.NowStory,
            _line = GameManager.Nani.ScriptPlayer.PlayedIndex,
            _likeability = int.Parse(GameManager.Nani.VarManager.GetVariableValue("Likeability"))
        };

        Debug.Log("에피소드" + data._episod);
        Debug.Log(data._line + "번째 줄");
        Debug.Log("호감도" + data._likeability);

        if (_userData.dataList.ContainsKey(index))
        {
            _userData.dataList[index] = data;
        }
        else
        {
            _userData.dataList.Add(index, data);
        }

        Debug.Log(index + "번 인덱스");
        SaveUserData();
        Debug.Log("현재 저장된 스토리 개수" + _userData.dataList.Count);
    }

    public void LoadScriptData(int index)
    {
        if (_userData.dataList.ContainsKey(index))
        {
            ScriptData2 data = _userData.dataList[index];
            // 해당하는 데이터로 스토리 재생
        }
        else
        {
            Debug.Log("해당 인덱스에 데이터가 없습니다.");
        }
    }

    // 게임별 난이도별 점수를 저장하는 메서드
    public void SaveScore(int game, int difficulty, int score)
    {
        if (!_userData.scoreByGameAndDifficulty.ContainsKey(game))
        {
            _userData.scoreByGameAndDifficulty[game] = new Dictionary<int, int>();
        }
        _userData.scoreByGameAndDifficulty[game][difficulty] = score;
        SaveUserData();
    }

    // 게임별 난이도별 점수를 불러오는 메서드
    public int LoadScore(int game, int difficulty)
    {
        if (_userData.scoreByGameAndDifficulty.ContainsKey(game) && _userData.scoreByGameAndDifficulty[game].ContainsKey(difficulty))
        {
            return _userData.scoreByGameAndDifficulty[game][difficulty];
        }
        return 0;
    }
}

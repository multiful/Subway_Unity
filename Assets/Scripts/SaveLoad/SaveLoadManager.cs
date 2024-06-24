using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
class UserData
{
    public int _lastEpisod { get; set; }
    public int _lastLine { get; set; }
    public int _ticket { get; set; }
    public int _money { get; set; }

    public List<int> _ending;

    // 일기장, 안약 등 더 추가할 데이터

    public Dictionary<int, ScriptData> dataList = new Dictionary<int, ScriptData>();

}
[Serializable]
class ScriptData
{
    public int _episod { get; set; }
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
    //현재 매니저에 장착된 유저데이터
    private static UserData _userData;

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
        Debug.Log("저장");
        PlayerPrefs.SetString("GameData", DataToJson(_userData));
    }
    //겜 시작할때 불러줘야 함
    public void LoadUserData()
    {
         if(!PlayerPrefs.HasKey("GameData"))
         {
            Debug.Log("게임 데이터 없음");
            _userData = new UserData();
         }
         else
         {
            Debug.Log("게임 데이터 존재");
            _userData = JsonToData<UserData>(PlayerPrefs.GetString("GameData"));
         }
    }
    public void SaveScriptData(int index)
    {
        ScriptData data = new ScriptData();
        //현재 에피소드 기록
        //data._episod = 
        //현재 라인 기록
        //data._line = 
        //현재 호감도 기록
        //data._likeability = 
        _userData.dataList.Add(index, data);
        //유저 데이터 업데이트
        SaveUserData();
    }
    public void LoadScriptData(int index)
    {
        ScriptData data = new ScriptData();
        data = _userData.dataList[index];
        // 해당하는 데이터로 스토리 재생
    }

    private void Awake()
    {
        if(_Inst == null)
        {
            _Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadUserData();
        SaveUserData();
    }
}

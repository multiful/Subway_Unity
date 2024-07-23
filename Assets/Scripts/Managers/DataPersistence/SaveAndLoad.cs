using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class UserData3
{
    public string _userName { get; set; }
    public NowStoryName _lastEpisod { get; set; }
    public int _lastLine { get; set; }
    public int _ticket { get; set; }
    public int _money { get; set; }
    public int _eyeDrops { get; set; }
    public List<Diary> _diaryList { get; set; }
    public List<int> _ending { get; set; }

    public Dictionary<int, ScriptData> dataList = new Dictionary<int, ScriptData>();
}

[System.Serializable]
public class scriptData3
{
    public NowStoryName _episod { get; set; }
    public int _line { get; set; }
    public int _likeability { get; set; }
}

public class SaveAndLoad : MonoBehaviour
{


    private UserData saveData = new UserData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    
    // Start is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);


    }

    // 저장
    public void SaveData()
    {
        

        //saveData.playerPos = thePlayer.transform.position;
        //saveData.playerPos = thePlayer.transform.eulerAngles;

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);

    }
    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {


            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);

            saveData = JsonUtility.FromJson<UserData>(loadJson);

            

            //thePlayer.treansform.position = saveData.playerPos;
            //thePlayer.treansform.eulerAngles = saveData.playerRot;

            Debug.Log("로드 완료");
        }
        else
            Debug.Log("세이브 파일이 없습니다.");
    }
}


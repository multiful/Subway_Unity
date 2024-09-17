using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    public static UserData userData;

    public void SaveData() => DataToJSON();
    public void LoadData() => DataFromJSON();

    public void Init()
    {
        //LoadData();
        DataInit();
    }

    public void DataToJSON()
    {

    }

    public void DataFromJSON()
    {
        string path = Path.Combine(Application.persistentDataPath, "userData.json");
        if (!File.Exists(path))
        {
            userData = new UserData();
            userData.Init();
            SaveData();
        }
        string jsonData = File.ReadAllText(path);
        userData = JsonUtility.FromJson<UserData>(jsonData);
    }
    public void DataInit()
    {
        string path = Path.Combine(Application.persistentDataPath, "userData.json");
        userData = new UserData();
        userData.Init();
        string jsonData = JsonUtility.ToJson(userData);
        File.WriteAllText(path, jsonData);
    }
}

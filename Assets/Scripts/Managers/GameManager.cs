using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager s_instance;

    public static GameManager Inst { get { Init(); return s_instance; } }

    NaniEngineManager _nani = new NaniEngineManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui_manager = new UIManager();
    SoundManager _sound = new SoundManager();
    DataManager _data = new DataManager();
    //NicknameManager _nickname = new NicknameManager();
    //SceneMManager _scene = new SceneMManager();

    public static NaniEngineManager Nani { get { return Inst._nani; } }
    public static ResourceManager Resource { get { return Inst._resource; } }
    public static UIManager UI { get { return Inst._ui_manager; } }
    public static SoundManager Sound { get { return Inst._sound; } }
    public static DataManager Data { get { return Inst._data; } }
    //public static NicknameManager NickName { get { return instance._nickname; } }
    //public static SceneMManager Scene { get { return instance._scene; } }

    void Awake()
    {
        Init();
    }


    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null)
            {
                go = new GameObject { name = "@GameManager" };
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();

            s_instance._nani.Init();
            s_instance._sound.Init();
            //s_instance._data.Init();
            //s_instance._scene.Init();
        }
    }

    public static void SaveGame(string slotName)
    {
        // 예시: 나니노벨 상태 저장
        var stateManager = Engine.GetService<IStateManager>();
        stateManager.SaveGameAsync(slotName);
        Debug.Log($"Game saved in slot: {slotName}");
    }

    public static void LoadGame(string slotName)
    {
        // 예시: 나니노벨 상태 불러오기
        var stateManager = Engine.GetService<IStateManager>();
        stateManager.LoadGameAsync(slotName);
        Debug.Log($"Game loaded from slot: {slotName}");
    }

    public static void Clear()
    {
        Sound.Clear();
        UI.Clear();
    }
}

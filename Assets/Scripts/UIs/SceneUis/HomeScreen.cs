using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Naninovel;

public class HomeScreen : MonoBehaviour
{
    private GameObject _BackGroundObject;
    public static HomeScreen Inst;
    public TMP_Text TMP_NowStory;

    private void Awake()
    {
        if (Inst == null) Inst = this;
        else Destroy(this);

        //Trasfer 실행할때 배경 사라지게 함
        //다른 UI들 어디서 꺼지는지 몰라서 일단 여기 박아뒀는데 다른 UI들이랑 묶어서 수정해야함
        _BackGroundObject = GameObject.Find("MainBackGround");
    }
   
    
    [ContextMenu("업데이트 갱신")]
    public void UpdateUIs()
    {
        TMP_NowStory.text = GameManager.Data.NowStory.ToString().UnderscoreToSpace();
    }

    public async void Transfer()
    {
        string ScriptName = GameManager.Data.NowStory.ToString();
    
        // 2. Switch cameras.
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;

        // 3. Load and play specified script (if assigned).
        await GameManager.Nani.ScriptPlayer.PreloadAndPlayAsync(ScriptName);

        // 4. Enable Naninovel input.
        GameManager.Nani.InputManager.ProcessInput = true;

        //이거 어디로 옮겨야함? 스크립트 시작하면 다른 UI들 어디서 비활성화 되는지 모르겠음
        _BackGroundObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateUIs();
    }
}

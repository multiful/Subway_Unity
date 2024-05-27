using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Naninovel;

public class HomeScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _BackGroundObject;
    public static HomeScreen Inst;
    public TMP_Text TMP_NowStory;

    private void Awake()
    {
        if (Inst == null) Inst = this;
        else Destroy(this);
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
        _BackGroundObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateUIs();
    }
}

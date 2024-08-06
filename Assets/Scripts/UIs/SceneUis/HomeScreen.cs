using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Naninovel;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public GameObject _BackGroundObject;
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

        //이거 어디로 옮겨야함? 스크립트 시작하면 다른 UI들 어디서 비활성화 되는지 모르겠음
        _BackGroundObject.SetActive(false);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _BackGroundObject.SetActive(true);
        UpdateUIs();
    }

    public void ShowSettingUI()
    {
        GameManager.UI.ShowPopupUI<UI_Setting>();
    }
    public void GoShop()
    {
        SceneManager.LoadScene("Shop");
    }
    private void Start()
    {
        if (!GameManager.Sound.isBGMPlaying())
        {
            GameManager.Sound.Play("MainTheme", Sound.Bgm);
        }
    }
}

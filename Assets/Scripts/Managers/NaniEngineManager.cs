using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaniEngineManager
{
    public Camera MainCamera;
    public IInputManager InputManager;
    public IStateManager StateManager;
    public IScriptPlayer ScriptPlayer;
    public ICustomVariableManager VarManager;
    public IUIManager UIManager;
    public IAudioManager AudioManager;

    public void Init()
    {
        // Engine may not be initialized here, so check first.
        if (Engine.Initialized) InitProcess();
        else Engine.OnInitializationFinished += InitProcess;
    }

    private void InitProcess()
    {
        CacheEngineServices();
        
        //메인 카메라 설정 변경
        MainCamera.clearFlags = CameraClearFlags.Depth;
    }

    /// <summary>
    /// 자주 사용되는 엔진서비스들을 미리 캐싱해둠
    /// </summary>
    void CacheEngineServices()
    {
        MainCamera   = Engine.GetService<ICameraManager>().Camera;
        InputManager = Engine.GetService<IInputManager>();
        StateManager = Engine.GetService<IStateManager>();
        ScriptPlayer = Engine.GetService<IScriptPlayer>();
        VarManager = Engine.GetService<ICustomVariableManager>();
        UIManager = Engine.GetService<IUIManager>();
        AudioManager = Engine.GetService<IAudioManager>();

        var audioMixer = AudioManager.AudioMixer;
    }

    public async void PlayNani(string scriptName)
    {
        GameManager.Sound.Clear();

        // 2. Switch cameras.
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;

        // 3. Load and play specified script (if assigned).
        await GameManager.Nani.ScriptPlayer.PreloadAndPlayAsync(scriptName);

        // 4. Enable Naninovel input.
        GameManager.Nani.InputManager.ProcessInput = true;
    }
}


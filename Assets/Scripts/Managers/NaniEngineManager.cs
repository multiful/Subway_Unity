using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaniEngineManager
{
    public ICameraManager CameraManager;
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
        CameraManager.Camera.clearFlags = CameraClearFlags.Depth;
    }

    /// <summary>
    /// 자주 사용되는 엔진서비스들을 미리 캐싱해둠
    /// </summary>
    void CacheEngineServices()
    {
        CameraManager = Engine.GetService<ICameraManager>();
        InputManager = Engine.GetService<IInputManager>();
        StateManager = Engine.GetService<IStateManager>();
        ScriptPlayer = Engine.GetService<IScriptPlayer>();
        VarManager = Engine.GetService<ICustomVariableManager>();
        UIManager = Engine.GetService<IUIManager>();
        AudioManager = Engine.GetService<IAudioManager>();

        var audioMixer = AudioManager.AudioMixer;
    }

    public async void PlayNani(string scriptName, string label = null, StoryType type = StoryType.일반스토리)
    {
        GameManager.Sound.Clear(); // ?

        // 2. Switch cameras.
        CameraManager.Camera.enabled = true;

        // 3. Load and play specified script (if assigned).
        switch (type)
        {
            case StoryType.일반스토리:
                scriptName = "일반스토리" + scriptName; break;
            case StoryType.미니게임:
                scriptName = "" + scriptName; break;
        }

        await (label != null ? ScriptPlayer.PreloadAndPlayAsync(scriptName, label : label)
            : ScriptPlayer.PreloadAndPlayAsync(scriptName));

        // 4. Enable Naninovel input.
        InputManager.ProcessInput = true;
    }

    public async void StopNani()
    {
        // 1. Disable Naninovel input.
        InputManager.ProcessInput = false;

        // 2. Stop script player.
        ScriptPlayer.Stop();

        // 3. Reset state.
        await StateManager.ResetStateAsync();

        // 4. Switch cameras.
        CameraManager.Camera.enabled = false;

    }
}


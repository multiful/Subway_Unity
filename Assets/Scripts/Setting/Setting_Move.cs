using Naninovel;
using Naninovel.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Move : MonoBehaviour
{
    private GameSettingsMenu settingsMenu;
    private IStateManager settingsManager;

    private void Awake()
    {
        settingsMenu = GetComponentInParent<GameSettingsMenu>();
        settingsManager = Engine.GetService<IStateManager>();
    }
    private void CloseSetting()
    {
        settingsManager.SaveSettingsAsync();
        settingsMenu.Hide();
    }
    public void FinishStory()
    {
        CloseSetting();
        // 1. Disable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = false;

        // 2. Stop script player.
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        scriptPlayer.Stop();

        // 3. Reset state.
        GameManager.Nani.StateManager.ResetStateAsync();

        // 4. Switch cameras.
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = false;

        MainScreen.Inst.gameObject.SetActive(true);
    }
    public void QuitGame()
    {
        CloseSetting();
        Application.Quit();
    }
}

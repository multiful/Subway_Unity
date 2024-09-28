using Naninovel;
using Naninovel.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Setting_Move : MonoBehaviour
{
    private GameSettingsMenu settingsMenu;
    private IStateManager settingsManager;
    public Button mainButton;
    private void Awake()
    {
        settingsMenu = GetComponentInParent<GameSettingsMenu>();
        settingsManager = Engine.GetService<IStateManager>();
        if(SceneManager.GetActiveScene().name == "Main")
        {
            mainButton.interactable = false;
        }
        else
        {
            mainButton.interactable = true;
        }
    }
    private void CloseSetting()
    {
        settingsManager.SaveSettingsAsync();
        settingsMenu.Hide();
    }
    public void FinishStory()
    {
        CloseSetting();
        GameManager.Nani.StopNani();
        LoadingSceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        CloseSetting();
        Application.Quit();
    }
}

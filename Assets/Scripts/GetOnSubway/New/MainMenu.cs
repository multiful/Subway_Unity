using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        // 버튼 상태 설정
        DisableButtonsDependingOnData();
    }

    // 저장된 게임 데이터가 있는지 확인하고, 없으면 continueGameButton과 loadGameButton을 비활성
    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
    // 새로운 게임을 클릭했을 때 호출
    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    // 저장된 게임을 불러오는 버튼을 클릭했을 때 호출
    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    // 기존 게임을 계속하는 버튼을 클릭했을 때 호출
    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("SampleScene");
    }


    // 메뉴 버튼 비활성
    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }


    // 메뉴 버튼 활성
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }


    // 메뉴 비활성.
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}

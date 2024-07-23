using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    // 메인 메뉴 객체
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    // 뒤로 가기 버튼
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;


    // 화깅ㄴ 팝업 메뉴를 나타내는 객체
    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;


    // 저장 슬롯 배열
    private SaveSlot[] saveSlots;

    // 게임을 불러오고 있는지 여부를 나타내는 bool
    private bool isLoadingGame = false;

    // 저장 슬롯 초기화
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    // 저장 슬롯 클릭시 호출
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        // disable all buttons
        DisableMenuButtons();

        // case - loading game
        if (isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            SaveGameAndLoadScene();
        }
        // case - new game, but the save slot has data
        else if (saveSlot.hasData)
        {
            confirmationPopupMenu.ActivateMenu(
                "Starting a New Game with this slot will override the currently saved data. Are you sure?",
                // function to execute if we select 'yes'
                () => {
                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                    DataPersistenceManager.instance.NewGame();
                    SaveGameAndLoadScene();
                },
                // function to execute if we select 'cancel'
                () => {
                    this.ActivateMenu(isLoadingGame);
                }
            );
        }
        // case - new game, and the save slot has no data
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            SaveGameAndLoadScene();
        }
    }

    // 저장 슬롯 데이터 삭제 때 호출

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmationPopupMenu.ActivateMenu(
            "Are you sure you want to delete this saved data?",
            // function to execute if we select 'yes'
            () => {
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());
                ActivateMenu(isLoadingGame);
            },
            // function to execute if we select 'cancel'
            () => {
                ActivateMenu(isLoadingGame);
            }
        );
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    // 메뉴를 활성화 하고 저장 슬롯의 상태를 설정.
    public void ActivateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;



        // load all of the profiles that exist
        Dictionary<string, UserData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        // ensure the back button is enabled when we activate the menu

        // loop through each save slot in the UI and set the content appropriately
        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots)
        {
            UserData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    // 메뉴 비활성
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    // 게임저장하고 새로운 씬 로드
    private void SaveGameAndLoadScene()
    {
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // load the scene
        SceneManager.LoadSceneAsync("SampleScene");
    }

    // 모든 메뉴 버튼 비활성
    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}


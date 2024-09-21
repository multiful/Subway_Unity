using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiscSettings : MonoBehaviour
{
    public Button helpButton;
    public Button creditsButton;
    public Button mainMenuButton;
    public Button quitButton;

    private void Start()
    {
        // 버튼 클릭 이벤트 설정
        helpButton.onClick.AddListener(OpenHelp);
        creditsButton.onClick.AddListener(OpenCredits);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OpenHelp()
    {
        // 도움말 화면으로 전환
        SceneManager.LoadScene("HelpScene");
    }

    private void OpenCredits()
    {
        // 크레딧 화면으로 전환
        SceneManager.LoadScene("CreditsScene");
    }

    private void GoToMainMenu()
    {
        // 메인 화면으로 이동
        SceneManager.LoadScene("Sample Scene");
    }

    private void QuitGame()
    {
        // 게임 종료
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Unity.VisualScripting;
using Naninovel;
using TMPro;

public class SettingsUIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject soundSettingsPanel;
    public GameObject textSettingsPanel;
    public GameObject miscSettingsPanel;
    public Button soundButton;
    public Button textButton;
    public Button miscButton;
    public Button exitButton;

    public AudioMixer audioMixer; // 오디오 믹서 참조

    private void Start()
    {
        // 버튼 클릭 이벤트 리스너 설정
        soundButton.onClick.AddListener(() => ShowPanel(soundSettingsPanel, soundButton));
        soundButton.onClick.AddListener(() => SetTextMode(0));

        textButton.onClick.AddListener(() => ShowPanel(textSettingsPanel, textButton));
        textButton.onClick.AddListener(() => SetTextMode(1));

        miscButton.onClick.AddListener(() => ShowPanel(miscSettingsPanel, miscButton));
        miscButton.onClick.AddListener(() => SetTextMode(2));
        exitButton.onClick.AddListener(CloseSettings);

        // 초기에는 사운드 설정 패널을 보여줌
        ShowPanel(soundSettingsPanel, soundButton);
        SetTextMode(0);
    }

    // 선택한 버튼의 텍스트를 굵게 설정
    private void SetTextMode(int mode)
    {
        // 모드 저장
        PlayerPrefs.SetInt("TextMode", mode);

        // 버튼 스타일 변경
        soundButton.GetComponentInChildren<TMP_Text>().fontStyle = mode == 0 ? FontStyles.Bold : FontStyles.Normal;
        textButton.GetComponentInChildren<TMP_Text>().fontStyle = mode == 1 ? FontStyles.Bold : FontStyles.Normal;
        miscButton.GetComponentInChildren<TMP_Text>().fontStyle = mode == 2 ? FontStyles.Bold : FontStyles.Normal;
    }

    private void ShowPanel(GameObject panelToShow, Button selectedButton)
    {
        // 모든 패널 비활성화
        soundSettingsPanel.SetActive(false);
        textSettingsPanel.SetActive(false);
        miscSettingsPanel.SetActive(false);

        // 선택한 패널 활성화
        panelToShow.SetActive(true);

        // 모든 버튼의 텍스트를 일반으로 설정
        

        // BGM 일시 정지
        audioMixer.SetFloat("BGMVolume", -80); // BGM 볼륨을 최소로 설정하여 일시정지 효과
        // AudioManager.Instance.PauseAllBGM();
    }

    public void CloseSettings()
    {
        Debug.Log("Close Setting");
        soundSettingsPanel.SetActive(false);
        textSettingsPanel.SetActive(false);
        miscSettingsPanel.SetActive(false);

        // BGM 재생 재개
        audioMixer.SetFloat("BGMVolume", 0);
        // AudioManager.Instance.ResumeAllBGM();

        // 설정 창 닫기
        settingsPanel.SetActive(false);
    }
}

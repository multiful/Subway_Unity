using Naninovel;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIOn : MonoBehaviour
{
    public Button settingsButton;

    async void Start()
    {
        // 나니노벨 엔진이 초기화될 때까지 기다림
        await RuntimeInitializer.InitializeAsync();

        // 버튼에 리스너 추가
        settingsButton.onClick.AddListener(OpenSettingsUI);
    }

    void OpenSettingsUI()
    {
        // 나니노벨의 Settings UI 호출
        var settingsUI = Engine.GetService<IUIManager>().GetUI<ISettingsUI>();
        settingsUI.Show();
    }
}

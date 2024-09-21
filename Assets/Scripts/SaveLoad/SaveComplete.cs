using Naninovel;
using Naninovel.UI;
using UnityEngine;

public class SaveComplete : MonoBehaviour
{
    private IStateManager stateManager;
    private IUIManager uiManager;

    private void Awake()
    {
        stateManager = Engine.GetService<IStateManager>();
        uiManager = Engine.GetService<IUIManager>();

        // 세이브가 완료되었을 때 호출되는 이벤트에 커스텀 Toast 연결
        stateManager.OnGameSaveFinished += OnSaveComplete;
    }

    private void OnSaveComplete(GameSaveLoadArgs args)
    {
        // Toast UI 불러오기
        var toastUI = uiManager.GetUI<IToastUI>();
        toastUI.Show($"세이브가 완료되었습니다.");
    }
}

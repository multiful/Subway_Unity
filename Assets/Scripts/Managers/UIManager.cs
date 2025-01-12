using JetBrains.Annotations;
using Naninovel;
using Naninovel.Commands;
using Naninovel.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager
{
    int _order = 0;

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _scene = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);
        go.transform.SetParent(Root.transform);
        return popup;
    }
    public void ShowSettingUI()
    {
        var settingsUI = Engine.GetService<IUIManager>().GetUI<ISettingsUI>();
        
        settingsUI.Show();
    }
    public UI_EndingPlay ShowEndingPlayUI(EndingSO so)
    {
        GameObject go = GameManager.Resource.Instantiate("UI/Popup/UI_EndingPlay");
        UI_EndingPlay popup = Util.GetOrAddComponent<UI_EndingPlay>(go);
        popup.SetEndingTexts(so);
        _popupStack.Push(popup);
        go.transform.SetParent(Root.transform);
        return popup;
    }
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;
        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }
        ClosePopupUI();
    }
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;
        UI_Popup popup = _popupStack.Pop();
        GameManager.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode= RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
            canvas.sortingOrder = 5 + _order++;
        else
            canvas.sortingOrder = -2;
    }
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _scene = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }
    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);


        return Util.GetOrAddComponent<T>(go);
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
    }

    /* 해상도 설정하는 함수 */
    //public void SetResolution()
    //{
    //    Debug.Log("해상도 설정");
    //    int setWidth = 2960; // 사용자 설정 너비
    //    int setHeight = 1440; // 사용자 설정 높이

    //    int deviceWidth = Screen.width; // 기기 너비 저장
    //    int deviceHeight = Screen.height; // 기기 높이 저장

    //    Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

    //    if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
    //    {
    //        float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
    //        Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
    //    }
    //    else // 게임의 해상도 비가 더 큰 경우
    //    {
    //        float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
    //        Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
    //    }
    //}
}

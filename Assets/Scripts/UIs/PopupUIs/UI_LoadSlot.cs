using Naninovel.UI;
using Naninovel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadSlot : MonoBehaviour
{
    public void CloseLoadUI()
    {
        Debug.Log("닫아졌냐");
        var loadUI = Engine.GetService<IUIManager>().GetUI<ISaveLoadUI>();
        if (loadUI is null) return;
        loadUI.Hide();
    }
}

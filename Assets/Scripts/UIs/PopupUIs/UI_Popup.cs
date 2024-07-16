using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        GameManager.UI.SetCanavas(gameObject, true);
    }
    public void Close()
    {
        GameManager.UI.ClosePopupUI(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Credit : UI_Popup
{
    enum Buttons { CloseButton }
    Button closeBtn;

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        closeBtn = Get<Button>((int)Buttons.CloseButton);
        closeBtn.onClick.AddListener(Close);
    }
    private void Awake()
    {
        Init();
    }
}

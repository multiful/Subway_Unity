using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel.UI;
using Naninovel;
public class CreditUI : MonoBehaviour
{
    public void ShowCredit()
    {
        GameManager.UI.ShowPopupUI<UI_Credit>();
    }
}


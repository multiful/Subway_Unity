using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationToggle : MonoBehaviour
{
    public Text text;
    public Toggle toggle;

    public void TextBold()
    {
        if(toggle.isOn)
        {
            text.fontStyle = FontStyle.Bold;
        }
        else
        {
            text.fontStyle = FontStyle.Normal;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst : MonoBehaviour
{
    public Canvas can;
    // Start is called before the first frame update
    void Start()
    {
        UI_Popup go = GameManager.UI.ShowPopupUI<UI_Popup>("Image");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

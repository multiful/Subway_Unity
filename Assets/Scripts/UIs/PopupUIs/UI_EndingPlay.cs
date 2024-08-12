using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EndingPlay : UI_Popup
{
    enum Texts
    {
        Ending_Name,
        Ending_Type,
        Ending_Info
    }
    enum Buttons
    {
        Close,
        Ending_Play
    }
    TextMeshProUGUI _name;
    TextMeshProUGUI _type;
    TextMeshProUGUI _info;

    Button _play;
    public void SetEndingTexts(int index)
    {
        Ending ending = (Ending)(index);
        EndingType endingType = (EndingType)(index);

        _name.text = ending.ToString();
        _type.text = endingType.ToString();

        _info.text = $"{index+1}번 엔딩 줄거리";
        _play.onClick.AddListener(() => EndingPlay(index));
    }
    private void EndingPlay(int index)
    {
        Debug.Log($"{index+1}번 엔딩 플레이");
    }
    public override void Init()
    {
        base.Init();
        Bind<TextMeshProUGUI>(typeof(Texts));
        _name = Get<TextMeshProUGUI>((int)Texts.Ending_Name);
        _type = Get<TextMeshProUGUI>((int)Texts.Ending_Type);
        _info = Get<TextMeshProUGUI>((int)Texts.Ending_Info);

        Bind<Button>(typeof(Buttons));
        Button _close = Get<Button>((int)Buttons.Close);
        _play = Get<Button>((int)Buttons.Ending_Play);

        _close.onClick.AddListener(Close);
    }
    private void Awake()
    {
        Init();
    }
    void Start()
    {

    }
}

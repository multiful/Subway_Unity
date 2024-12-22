using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Help : UI_Popup
{
    enum Buttons
    {
        LeftButton,
        RightButton
    }
    enum Images
    {
        HelpImage
    }
    [SerializeField]
    private List<Image> _helpImageList;

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Button _leftButton = Get<Button>((int)Buttons.LeftButton);
        Button _rightButton = Get<Button>((int)Buttons.RightButton);

        Bind<Image>(typeof(Images));
        Image _helpImage = Get<Image>((int)Images.HelpImage);
    }
    private void Awake()
    {
        Init();
    }
    private void ImageMoveButton(int n)
    {

    }
}

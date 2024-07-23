using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    public Button leftButton;
    public Button rightButton;
    public PlayerController1 playerController;

    void Start()
    {
        leftButton.onClick.AddListener(OnLeftButtonClicked);
        rightButton.onClick.AddListener(OnRightButtonClicked);
    }

    void OnLeftButtonClicked()
    {
        playerController.MoveLeft();
    }

    void OnRightButtonClicked()
    {
        playerController.MoveRight();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ControlPanelMover : MonoBehaviour
{
    public Button menuButton;
    public Button closeButton;
    public GameObject controlPanel;

    public float moveDuration = 0.5f;
    public void MenuMove()
    {
        controlPanel.transform.DOLocalMoveX(0, moveDuration);
        closeButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(false);
    }
    public void MenuClose()
    {
        controlPanel.transform.DOLocalMoveX(440, moveDuration);
        closeButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(true);
    }
    private void Start()
    {
        
    }
}

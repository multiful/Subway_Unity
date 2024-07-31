using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Naninovel;

public class TestBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void scriptStart()
    {
        // 카메라 설정
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;

        // 스크립트 불러오기
        await GameManager.Nani.ScriptPlayer.PreloadAndPlayAsync("카드뒤집기_성공");

        // Enable Naninovel inout
        GameManager.Nani.InputManager.ProcessInput = true;

        gameObject.SetActive(false);
    }

    public void MainCanvasBtn()
    {
        Debug.Log("Main");
    }
    public void SubCanvasBtn()
    {
        Debug.Log("Sub");
    }
}

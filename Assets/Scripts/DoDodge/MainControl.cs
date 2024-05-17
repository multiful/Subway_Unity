using JetBrains.Annotations;
using Naninovel;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainControl: MonoBehaviour //전체 두더지게임 관리
{
    private float _currentTime = 59.9f;

    public static MainControl Inst; //temporary singleton
    public TMP_Text TMP_Time;
    public int womanSeat = -1; //여자가 아직 안 앉은 상태
    public delegate void stopGame();
    public static event stopGame StopGame;

    public GameObject GoodClearCanvas, BadClearCanvas, FailCanvas; //temp

    private IEnumerator StartTimer()
    {
        while (_currentTime > 0f) //제한시간 60초
        {
            TMP_Time.text = ((int)_currentTime + 1).ToString(); //경과 시간 표시

            _currentTime -= Time.deltaTime;
            yield return null;
        }
        Fail(); //60초 초과 시 실패
    }

    private bool IsNextSeat(int seat1, int seat2)
    {
        if (Mathf.Abs(seat1 - seat2) == 1) return true;
        return false;
    }

    
    private IEnumerator GoodClear()
    {
        yield return new WaitForSeconds(2f);
        GoodClearCanvas.SetActive(true);
    }

    private IEnumerator BadClear()
    {
        yield return new WaitForSeconds(2f);
        BadClearCanvas.SetActive(true);
    }

    private void Fail()
    {
        StopGame();
        TMP_Time.text = "0";
        FailCanvas.SetActive(true);
    }

    public void Clear(int manSeat)
    {
        StopAllCoroutines(); //타이머 종료
        StopGame(); //이벤트 델리게이트로 모든 버튼 비활성화
        if (IsNextSeat(womanSeat, manSeat)) StartCoroutine(GoodClear());
        else StartCoroutine(BadClear());
    }


    public void Penalty(float time)
    {
        _currentTime -= time;
    }


    void Start()
    {
        if (Inst == null) Inst = this;
        else Destroy(this); //temporary singleton

        StartCoroutine(StartTimer());
    }

}

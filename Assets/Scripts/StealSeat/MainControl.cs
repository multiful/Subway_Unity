using JetBrains.Annotations;
using Naninovel;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class MainControl : MonoBehaviour //전체 두더지게임 관리
{

    public static MainControl Inst; //temporary singleton
    public TMP_Text TMP_Time;
    public int womanSeat = -1; //여자가 아직 안 앉은 상태
    public delegate void stopGame();
    public static event stopGame StopGame;

    public List<int> occupiedList = new List<int>(); //0~9 사이 앉은 자리 7개
    public List<int> unoccupiedList = new List<int>(); //그 외 남는 자리 3개

    public GameObject GoodClearCanvas, BadClearCanvas, FailCanvas; //temp

    public int gameLevel = 0;
    private int[] timeLimit = { 60, 60, 50, 50 };
    private float _currentTime;
    private int reward;

    public TMP_Text rewardText1, rewardText2; //temp

    void Awake()
    {
        if (Inst == null) Inst = this;
        else Destroy(this); //temporary singleton

        InitArray();

        _currentTime = timeLimit[gameLevel];
        StartCoroutine(StartTimer());
    }

    private void InitArray() //스프라이트 번호 나열
    {
        for (int i = 0; i <= 20; i++)
        {
            unoccupiedList.Add(i); //0~20
        }
        for (int i = 0; i <= 6; i++) //7자리 채우기
        {
            int rand = Random.Range(0, unoccupiedList.Count);
            occupiedList.Add(unoccupiedList[rand]);
            unoccupiedList.RemoveAt(rand);
        }
    }

    private IEnumerator StartTimer()
    {
        while (_currentTime > 0f) //제한시간 timeLevel[gameLevel]초
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
        rewardText1.text = "보상 : " + reward.ToString() + "원, 호감도 +5";
        GoodClearCanvas.SetActive(true);
    }

    private IEnumerator BadClear()
    {
        yield return new WaitForSeconds(2f);
        rewardText2.text = "보상 : " + reward.ToString() + "원";
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
        reward = 500 * ((int)_currentTime + 1);

        if (IsNextSeat(womanSeat, manSeat)) StartCoroutine(GoodClear());
        else StartCoroutine(BadClear());
    }

    public void Penalty(float time)
    {
        _currentTime -= time;
    }

    public int GetSeat(int prevNum)
    {
        int rand = Random.Range(0, unoccupiedList.Count);
        int num = unoccupiedList[rand];
        occupiedList.Add(num); //새로운 사람
        unoccupiedList.RemoveAt(rand);

        occupiedList.Remove(prevNum); //이전 사람 재착석 방지
        unoccupiedList.Add(prevNum);
        return num; //새로운 사람 스프라이트 번호 전달
    }

}

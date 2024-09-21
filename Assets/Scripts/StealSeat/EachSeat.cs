using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EachSeat : MonoBehaviour //각 버튼별 독립 동작
{

    private enum State
    {
        Sit, Fake, Stand, Woman, Man
    }
    private State _currentState = State.Sit;

    [SerializeField] private int seatNum;
    private int spriteNum;

    private readonly float[] changeInterval = { 1.5f, 1f, 1f, 0.7f };
    private float _currentInterval;


    void Start()
    {
        MainControl.StopGame += DeActivate; //델리게이트에 비활성 함수 추가
        StartCoroutine(Activate());

        spriteNum = MainControl.Inst.occupiedList[seatNum]; //스프라이트 번호 받기
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("npc/" + (spriteNum + 1).ToString() + ".1");
        _currentInterval = changeInterval[MainControl.Inst.gameLevel];
    }

    private IEnumerator Activate()
    {
        yield return new WaitForSeconds(Random.Range(3f, 7f)); //시작 시 3~7초 대기
        while (true)
        {
            float rand = Random.Range(0f, 1f);
            if (rand < 0.33f) StartCoroutine(Fake()); //3분의 1 확률로 페이크
            if (rand > 0.66f) StartCoroutine(Stand()); //3분의 1 확률로 일어남,
                                                       //남은 3분의 1 확률은 계속 앉아있음

            yield return new WaitForSeconds(Random.Range(3f, 7f)); //3~7초마다 위의 행동
        }
    }

    private void DeActivate() //버튼 비활성화
    {
        StopAllCoroutines();
        GetComponent<Button>().interactable = false;
    }

    private void Sit()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("npc/" + (spriteNum + 1).ToString() + ".1");
        _currentState = State.Sit;
    }

    private IEnumerator Fake()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("npc/" + (spriteNum + 1).ToString() + ".2");
        _currentState = State.Fake;
        yield return new WaitForSeconds(_currentInterval);
        Sit(); //1초 대기 후 다시 착석
    }

    private IEnumerator Stand()
    {
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("npc/" + (spriteNum + 1).ToString() + ".3");
        _currentState = State.Stand;
        yield return new WaitForSeconds(_currentInterval);

        spriteNum = MainControl.Inst.GetSeat(spriteNum); //1초 후 새로운 사람 착석
        Sit(); //0.5(임시 1초) 대기 후 다시 착석
    }

    public void ButtonClick() //0~6번 버튼마다 버튼 번호 전달받음
    {
        if (_currentState != State.Stand) //일어선 상태가 아니면
        {
            MainControl.Inst.Penalty(10f); //10초 페널티
            return;
        }

        DeActivate(); //버튼 고정
        if (MainControl.Inst.womanSeat == -1) //아직 여자가 안 앉았다면
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("npc/woman");
            MainControl.Inst.womanSeat = seatNum; //여자 좌석번호 전달
            _currentState = State.Woman;
        }
        else //여자가 이미 앉은 상태(게임 성공)
        {
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("npc/man");
            MainControl.Inst.Clear(seatNum); //남자 좌석번호 전달
            _currentState = State.Man;
        }

    }


}

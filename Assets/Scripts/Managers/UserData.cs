using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{

    public StoryName NowStoryName // 마지막으로 본 에피소드
    {
        get { return nowStoryName; }
        set
        {
            nowStoryName = value;
            GameManager.Data.SaveData();
        }
    }

    public int CurrentGameLevel
    {
        get { return currentGameLevel; }
        set
        {
            currentGameLevel = value;
            GameManager.Data.SaveData();
        }
    }

    //public int Ticket // 승차권
    //{
    //    get { return ticket; }
    //    set
    //    {
    //        LastTicketUsed = DateTime.Now;
    //        ticket = value;
    //        GameManager.Data.SaveData();
    //    }
    //}

    //public string TicketTime
    //{
    //    get
    //    {
    //        int sec = (DateTime.Now - LastTicketUsed).Seconds;
    //        if (sec >= 1200) Ticket++;

    //        sec = 1200 - sec;
    //        return (sec / 60).ToString() + " : " + (sec % 60).ToString();
    //    }
    //}

    public int Money // 돈
    {
        get { return money; }
        set
        {
            money = value;
            GameManager.Data.SaveData();
        }
    }

    public int Eyedrop // 안약
    {
        get { return eyedrop; }
        set
        {
            eyedrop = value;
            GameManager.Data.SaveData();
        }
    }

    //public DateTime LastTicketUsed // 마지막 티켓 사용 시각
    //{
    //    get { return lastTicketUsed; }
    //    set
    //    {
    //        lastTicketUsed = value;
    //        GameManager.Data.SaveData();
    //    }
    //}

    public int Diary // 일기장 해금 정도
    {
        get { return diary; }
        set
        {
            diary = value;
            GameManager.Data.SaveData();
        }
    }

    public bool[,] IsGameClear // 미니게임 클리어 여부
    {
        get { return isGameClear; }
        set
        {
            isGameClear = value;
            GameManager.Data.SaveData();
        }
    }

    //public int Progress // 진행도
    //{
    //    get //incompleted
    //    {
    //        return 50; // NowStoryName enum을 쓰고 싶으나 뒤죽박죽이다. 같은 스토리는 나니노벨 하나로 합쳤으면
    //    }
    //}

    public int LikeAbility // 호감도
    {
        get { return likeAbility; }
        set
        {
            likeAbility = Math.Min(100, value); // 퍼펙트로 선택지 골랐을때 딱 100이라면 무의미
            GameManager.Data.SaveData();
        }
    }

    public bool[] IsEndingUnlock // 엔딩 해금 여부
    {
        get { return isEndingUnlock; }
        set
        {
            isEndingUnlock = value;
            GameManager.Data.SaveData();
        }
    }

    private StoryName nowStoryName;
    private int currentGameLevel;
    //private int ticket;
    private int money;
    private int eyedrop;
    //private DateTime lastTicketUsed;
    private int diary;
    private bool[,] isGameClear;
    private int likeAbility;
    private bool[] isEndingUnlock;


    public void Init()
    {
        NowStoryName = StoryName.첫번째_등교;
        currentGameLevel = 0;
        Money = 0;
        //Ticket = 5;
        Eyedrop = 0;
        //LastTicketUsed = DateTime.Now;
        Diary = 0;
        IsGameClear = new bool[2, 4];
        LikeAbility = 0;
        IsEndingUnlock = new bool[4];
    }

}

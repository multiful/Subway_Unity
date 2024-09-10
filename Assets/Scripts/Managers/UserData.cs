using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string UserName; // 플레이어 이름
    public StoryName NowStoryName; // 마지막으로 본 에피소드

    public int CurrentGameLevel
    {
        get {
            switch (NowStoryName)
            {
                case StoryName.첫번째_등교: return 0;
                default: return 1;
            }
        }
    }

    public StoryName LastStoryName; //마지막으로 본 에피소드
    public int LastLine; // 마지막으로 본 대사
    public int Ticket; // 승차권
    public int Money; // 돈
    public int Eyedrop; // 안약
    public DateTime LastTicketUsed; // 마지막 티켓 사용 시각
    public int Diary; // 일기장 해금 정도
    public bool[,] IsEpisodeWatched; // 에피소드 열람 여부
    public bool[,] IsGameClear; // 미니게임 클리어 여부
    public int Progress; // 진행도
    public int LikeAbility; // 호감도
    public bool[] IsEndingUnlock; // 엔딩 해금 여부

    // 환경설정
    //public float MasterVolume;
    //public float BgmVolume;
    //public float SfxVolume;
    //public PrintTextMode PrintTextMode;
    //public int PrintTextSpeed;
    //public MainBGM MainBGM;


    public void Init()
    {
        UserName = string.Empty;
        NowStoryName = StoryName.첫번째_등교;
        LastStoryName = StoryName.None;
        LastLine = 0;
        Money = 0;
        Ticket = 5;
        Eyedrop = 0;
        LastTicketUsed = DateTime.MinValue;
        Diary = 0;
        IsEpisodeWatched = new bool[4, 4];
        IsGameClear = new bool[4, 4];
        Progress = 0;
        LikeAbility = 0;
        IsEndingUnlock = new bool[4];

        //MasterVolume = 1;
        //BgmVolume = 1;
        //SfxVolume = 1;
        //PrintTextMode = PrintTextMode.Touch;
        //PrintTextSpeed = 2;
        //MainBGM = MainBGM.MainTheme1;
    }

}

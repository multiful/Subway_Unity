using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public long lastUpdated;
    public string userName { get; set; } // 플레이어 이름
    public NowStoryName latest_ep { get; set; } // 마지막으로 본 에피소드
    public int latest_line { get; set; } // 마지막으로 본 대사.
    public int ticket { get; set; } // 승차권 개수
    public int money { get; set; } // 돈
    public int eyedrop { get; set; } // 안약 유무
    public DateTime lastTicketUsed { get; set; } // 마지막 티켓 사용 시각
    public List<Diary> diaryList { get; set; } // 일기장 해금 정도.
    public List<int> ending { get; set; } // 엔딩 해금 정도.

    // 게임과 난이도에 따른 클리어 여부를 저장
    public SerializableDictionary<int, SerializableDictionary<int, bool>> gameAndDifficultyCleared;

    public SerializableDictionary<int, ScriptData> dataList = new SerializableDictionary<int, ScriptData>();

    public UserData()
    {
        lastUpdated = 0;
        userName = string.Empty;
        latest_ep = new NowStoryName();
        latest_line = 0;
        ticket = 0;
        money = 0;
        eyedrop = 0;
        lastTicketUsed = DateTime.MinValue;
        diaryList = new List<Diary>();
        ending = new List<int>();
        gameAndDifficultyCleared = new SerializableDictionary<int, SerializableDictionary<int, bool>>();
        dataList = new SerializableDictionary<int, ScriptData>();
    }

    public int GetPercentageComplete()
    {
        return 0;
    }
}

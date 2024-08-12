using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NowStoryName
{
    첫번째_등교,
    두번째_드응교,
    세번째_등교오,
    부끄러워요,
    엔딩,
    MaxCount
}
public enum CardFlipEnding
{
    카드뒤집기_성공,
    카드뒤집기_실패
}
public enum Diary
{
    하지,
    설아,
    지철
}
public enum UIEvent
{
    Click,
    Drag,
}

public enum MouseEvent
{
    Press,
    Click,
}

public enum Sound
{
    Bgm,
    Effect,
    MaxCount
}

public enum MiniGame
{
    GetOnSubway,
    StealSeat,
    CardFlip
}

public enum Ending
{
    첫번째엔딩,
    두번째엔딩,
    세번째엔딩,
    네번째엔딩
}
public enum EndingType
{
    배드엔딩,
    노멀엔딩,
    트루엔딩
}
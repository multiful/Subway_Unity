using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StoryName
{
    None,
    첫번째_등교,
    e2,
    e3_1,
    e3_2,
    e4_1,
    e4_2,
    e5_1,
    e5_2,
    e6_1,
    e6_2,
    e7_1,
    e7_2,
    e8_1,
    e8_2,
    e9_1,
    e9_2,
    e10_1,
    e10_2,
    e11,
    e12,
    e13,
    e14,
    e15,
    e16,
    MaxCountPlusOne
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
public enum PrintTextMode
{
    Touch,
    Auto
}

public enum MainBGM
{
    MainTheme1,
    MainTheme2,
}
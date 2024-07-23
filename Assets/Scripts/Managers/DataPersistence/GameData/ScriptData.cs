using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptData
{
    public NowStoryName save_ep { get; set; } // 저장하는 ep.
    public int save_line { get; set; } // 저장하는 줄
    public int love_level { get; set; } // 호감도.

    public ScriptData()
    {
        save_ep = new NowStoryName();
        save_line = 0;
        love_level = 0;
    }

    public int GetPercentageComplete()
    {
        return 0;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    NowStoryName _nowStory;
    public NowStoryName NowStory { get { return _nowStory; } 
        set 
        { 
            _nowStory = value;
            if (HomeScreen.Inst.isActiveAndEnabled) HomeScreen.Inst.UpdateUIs();
        } 
    }
    
    [ContextMenu("스토리 처음으로 돌리기")]
    void ResetStory()
    {
        NowStory = NowStoryName.첫번째_등교;
    }
}

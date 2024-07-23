using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScriptDataPersistence
{
    void LoadData(ScriptData data);

    void SaveData(ScriptData data);


}

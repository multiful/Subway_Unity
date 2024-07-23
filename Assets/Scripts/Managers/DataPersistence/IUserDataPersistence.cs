using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserDataPersistence
{
    void LoadData(UserData data);

    void SaveData(UserData data);

    
}

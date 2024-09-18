using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnNaniStart : MonoBehaviour
{
    void Start()
    {
        GameManager.Nani.PlayNani(GameManager.userData.NowStoryName.ToString());
    }

}

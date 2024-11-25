using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveLoadSceneChanger : MonoBehaviour
{
    public void SceneChange()
    {
        if (SceneManager.GetActiveScene().name == "Main")
            LoadingSceneManager.LoadScene("Nani2");
    }
}

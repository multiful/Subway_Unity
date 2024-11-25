using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Naninovel;
using Naninovel.UI;
public class SaveLoadSceneChanger : MonoBehaviour
{
    public void SceneChange()
    {
        if (SceneManager.GetActiveScene().name == "Main")
            LoadingSceneManager.LoadScene("Nani2");

        var saveLoadUI = GameManager.Nani.UIManager.GetUI<ISaveLoadUI>();
        //saveLoadUI.Hide();
    }
}

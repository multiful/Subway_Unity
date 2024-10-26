using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Naninovel;
using UnityEngine.SceneManagement;
using Naninovel.UI;

public class MainScreen : MonoBehaviour
{
    public GameObject _BackGroundObject;
    public TMP_Text TMP_NowStory;
    public TMP_Text TMP_money, TMP_ticket, TMP_eyedrop;


    private void Start()
    {
        if (!GameManager.Sound.isBGMPlaying())
        {
            int randInt = Random.Range(0, 2);
            Debug.Log(randInt);
            if (randInt == 0)
            {
                GameManager.Sound.Play("subwayWayMainTheme1", Sound.Bgm);
            }
            else
            {
                GameManager.Sound.Play("subwayWayMainTheme2", Sound.Bgm);
            }
        }

        TMP_money.text = GameManager.userData.Money.ToString();
        TMP_eyedrop.text = GameManager.userData.Eyedrop.ToString();
    }

    private void Update()
    {
        //TMP_ticket.text = GameManager.userData.Ticket == 0 ?
        //    GameManager.userData.TicketTime : GameManager.userData.Ticket.ToString();
    }

    public void Transfer()
    {
        //GameManager.userData.Ticket--; // 티켓이 부족하면?

        LoadingSceneManager.LoadScene("Nani");
    }

    public void ShowSettingUI()
    {
        GameManager.UI.ShowSettingUI();
    }
    public void GoShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void ShowEndingCollection()
    {
        SceneManager.LoadScene(5);
    }
    public void MainSceenLoadUI()
    {
        var loadUI = Engine.GetService<IUIManager>().GetUI<ISaveLoadUI>();
        if (loadUI is null) return;
        loadUI.PresentationMode = SaveLoadUIPresentationMode.Load;
        loadUI.Show();
    }
    
}

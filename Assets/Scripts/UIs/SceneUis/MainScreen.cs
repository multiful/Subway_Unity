using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Naninovel;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public GameObject _BackGroundObject;
    public static MainScreen Inst;
    public TMP_Text TMP_NowStory;
    public TMP_Text TMP_money, TMP_ticket, TMP_eyedrop;

    private void Awake()
    {
        if (Inst == null) Inst = this;
        else Destroy(this);
    }

    private void Start()
    {
        if (!GameManager.Sound.isBGMPlaying())
        {
            GameManager.Sound.Play("MainTheme", Sound.Bgm);
        }


        TMP_money.text = GameManager.userData.Money.ToString();
        TMP_ticket.text = GameManager.userData.Ticket.ToString();
        TMP_eyedrop.text = GameManager.userData.Eyedrop.ToString();
    }

    [ContextMenu("업데이트 갱신")]
    public void UpdateUIs()
    {
        if (isActiveAndEnabled)
            TMP_NowStory.text = GameManager.userData.NowStoryName.ToString().UnderscoreToSpace();
    }

    public void Transfer()
    {
        GameManager.Sound.Clear();

        string ScriptName = GameManager.userData.NowStoryName.ToString();
        GameManager.Nani.PlayNani(ScriptName);

        _BackGroundObject.SetActive(false);

        gameObject.SetActive(false);

        GameManager.userData.Ticket--; //should modify
    }

    private void OnEnable()
    {
        _BackGroundObject.SetActive(true);
        UpdateUIs();
    }

    public void ShowSettingUI()
    {
        GameManager.UI.ShowPopupUI<UI_Setting>();
    }
    public void GoShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void ShowEndingCollection()
    {
        SceneManager.LoadScene(5);
    }
    
}

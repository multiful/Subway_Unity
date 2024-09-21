using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private int[] price = { 3000, 5000, 10000 };
    private int diaryUnlock = 0;
    public TMP_Text TMP_money, TMP_ticket, TMP_eyedrop;
    public GameObject[] itemZoom = new GameObject[3];
    public GameObject[] BehindDiary = new GameObject[3];
    public GameObject items, cardFlip;
    public Button setting;
    public TMP_Text speech;

    DateTime now;
    int timeLeft;

    public void TicketMinus()
    {
        GameManager.userData.Ticket--;
        TMP_ticket.text = GameManager.userData.Ticket.ToString();

        if (GameManager.userData.Ticket <= 0)
        {
            GameManager.userData.LastTicketUsed = DateTime.Now;
            StartCoroutine(TicketTimer());
        }
    }

    public IEnumerator TicketTimer()
    {
        DateTime last = GameManager.userData.LastTicketUsed;
        now = DateTime.Now;
        timeLeft = 20 - (int)(now - last).TotalSeconds;

        while (timeLeft > 0)
        {
            timeLeft--;

            string timeLeftText = (timeLeft / 60).ToString() + " : " + (timeLeft % 60).ToString();
            TMP_ticket.text = timeLeftText;

            yield return new WaitForSeconds(1);
        }

        GameManager.userData.Ticket++;
        TMP_ticket.text = GameManager.userData.Ticket.ToString();
    }

    private void Start()
    {

        TMP_money.text = GameManager.userData.Money.ToString();
        TMP_ticket.text = GameManager.userData.Ticket.ToString();
        TMP_eyedrop.text = GameManager.userData.Eyedrop.ToString();

        setting.onClick.AddListener(GameManager.UI.ShowSettingUI);

        speech.text = "ㅎㅇ";
    }

    public void ItemBuy(int itemNum)
    {
        if (GameManager.userData.Money >= price[itemNum]) //enough money
        {
            GameManager.userData.Money -= price[itemNum];
            TMP_money.text = GameManager.userData.Money.ToString();
            SuccessUI(itemNum);

            switch(itemNum) {
                case 0:
                    BehindDiary[diaryUnlock++].GetComponent<Image>().color = Color.white;
                    break; 
                case 1:
                    GameManager.userData.Ticket++;
                    StopAllCoroutines();
                    TMP_ticket.text = GameManager.userData.Ticket.ToString(); break;
                case 2:
                    GameManager.userData.Eyedrop++;
                    TMP_eyedrop.text = GameManager.userData.Eyedrop.ToString(); break;
            }
        }
        else NoMoneyUI();
    }

    private void SuccessUI(int itemNum)
    {
        itemZoom[itemNum].SetActive(false);
        speech.text = "ㄳ";
        items.SetActive(true);
        cardFlip.SetActive(true);
    }

    private void NoMoneyUI()
    {
        speech.text = "돈이 없다고? 물건은 팔 수 없겠어. 난 셈이 철저한 편이라.";
    }

    public void Diary(int diaryNum)
    {
        if (diaryUnlock <= diaryNum)
        {
            speech.text = "비밀에는 대가가 필요한 법...";
            return;
        }
        speech.text = "굿";
        //OnDiaryClick
    }

    public void GoCardFlip()
    {
        //SceneManager.LoadScene(MiniGame.CardFlip.ToString());
        LoadingSceneManager.LoadScene(MiniGame.CardFlip.ToString());
    }
    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }
}

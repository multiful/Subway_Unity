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

    private UserData userData = new UserData();

    DateTime now;
    int timeLeft;

    public void TicketMinus()
    {
        userData.ticket--;
        TMP_ticket.text = userData.ticket.ToString();

        if (userData.ticket <= 0)
        {
            userData.lastTicketUsed = DateTime.Now;
            StartCoroutine(TicketTimer());
        }
    }

    public IEnumerator TicketTimer()
    {
        DateTime last = userData.lastTicketUsed;
        now = DateTime.Now;
        timeLeft = 20 - (int)(now - last).TotalSeconds;

        while (timeLeft > 0)
        {
            timeLeft--;

            string timeLeftText = (timeLeft / 60).ToString() + " : " + (timeLeft % 60).ToString();
            TMP_ticket.text = timeLeftText;

            yield return new WaitForSeconds(1);
        }

        userData.ticket++;
        TMP_ticket.text = userData.ticket.ToString();
    }

    private void Awake()
    {
        userData.money = 20000;
        userData.ticket = 5;

        TMP_money.text = userData.money.ToString();
        TMP_ticket.text = userData.ticket.ToString();
        TMP_eyedrop.text = userData.eyedrop.ToString();

        setting.onClick.AddListener(GameManager.UI.ShowSettingUI);

        speech.text = "ㅎㅇ";
    }

    public void ItemBuy(int itemNum)
    {
        if (userData.money >= price[itemNum]) //enough money
        {
            userData.money -= price[itemNum];
            TMP_money.text = userData.money.ToString();
            SuccessUI(itemNum);

            switch(itemNum) {
                case 0:
                    BehindDiary[diaryUnlock++].GetComponent<Image>().color = Color.white;
                    break; 
                case 1:
                    userData.ticket++;
                    StopAllCoroutines();
                    TMP_ticket.text = userData.ticket.ToString(); break;
                case 2:
                    userData.eyedrop++;
                    TMP_eyedrop.text = userData.eyedrop.ToString(); break;
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
        SceneManager.LoadScene(MiniGame.CardFlip.ToString());
    }
    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }
}

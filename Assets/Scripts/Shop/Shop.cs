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
    public TMP_Text TMP_money, TMP_ticket, TMP_eyedrop;
    public GameObject noMoney, thankYou; //temp name
    public GameObject[] itemZoom = new GameObject[3];
    public GameObject items, cardFlip;
    public Button setting;

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
    }

    public void ItemBuy(int itemNum)
    {
        if (userData.money >= price[itemNum]) //enough money
        {
            userData.money -= price[itemNum];
            TMP_money.text = userData.money.ToString();
            SuccessUI(itemNum);

            switch(itemNum) {
                case 0: /*diary*/ break; 
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
        noMoney.SetActive(false);
        thankYou.SetActive(true);
        items.SetActive(true);
        cardFlip.SetActive(true);
    }

    private void NoMoneyUI()
    {
        thankYou.SetActive(false);
        noMoney.SetActive(true);
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

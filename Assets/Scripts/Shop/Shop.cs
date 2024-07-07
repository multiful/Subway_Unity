using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private int money = 20000; //temp
    private int[] price = { 3000, 5000, 10000 };
    public TMP_Text TMP_money;
    public GameObject noMoney, thankYou; //temp name
    public GameObject[] itemZoom = new GameObject[3];
    public GameObject items, cardFlip;

    private void Awake()
    {
        TMP_money.text = money.ToString();
    }

    public void ItemBuy(int itemNum)
    {
        if (money >= price[itemNum]) //enough money
        {
            money -= price[itemNum];
            TMP_money.text = money.ToString();

            itemZoom[itemNum].SetActive(false);
            noMoney.SetActive(false);
            thankYou.SetActive(true);
            items.SetActive(true);
            cardFlip.SetActive(true);
        }
        else //no money
        {
            thankYou.SetActive(false);
            noMoney.SetActive(true);
        }
    }
}

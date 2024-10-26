using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Naninovel;
using UnityEngine.SceneManagement;

public class CardFlipManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardSpriteList;

    public Transform cardSet;   //카드 덱 위치
    public Transform cardParent; //카드 프리팹 부모

    public TextMeshProUGUI buttonText;
    public Button startButton;
    public Button closeButton;

    public TextMeshProUGUI timerText;

    private List<Card> _cardList = new List<Card>();
    private Card _firstCard;
    private Card _secondCard;
    public bool _isCardChecking { get; private set; }
    public bool _isGameRunning = false;

    private int _matchedCard = 0;
    private float _gameTime = 60f;

    private CardFlipEnding _cardEnding;

    public Camera mainCam;

    public int reward;

    public Vector3[] cardPositions;
    public void CreateCards()
    {
        List<Sprite> cardImageList = new List<Sprite>(cardSpriteList);
        cardImageList.AddRange(cardSpriteList);

        // 카드 수 8개 제한
        while (cardImageList.Count > 8)
        {
            cardImageList.RemoveAt(cardImageList.Count - 1);
        }

        // 카드 섞기
        for (int i = 0; i < cardImageList.Count; i++)
        {
            int rand = Random.Range(0, cardImageList.Count);
            Sprite temp = cardImageList[i];
            cardImageList[i] = cardImageList[rand];
            cardImageList[rand] = temp;
        }

        // 카드 덱에 생성
        foreach (Sprite cardImage in cardImageList)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardSet.position, Quaternion.identity, cardParent);
            Card card = cardObject.GetComponent<Card>();
            card._frontSprite = cardImage;
            _cardList.Add(card);
        }
    }
    public void StartGame()
    {
        _matchedCard = 0;
        _gameTime = 60f;
        buttonText.text = "게임 진행중";
        startButton.interactable = false;
        StartCoroutine(MoveCards());
    }
    
    IEnumerator MoveCards()
    {
        float delay = 0.2f;
        for (int i = 0; i < _cardList.Count; i++)
        {
            Vector3 targetPosition = cardPositions[i];

            _cardList[i].transform.DOLocalMove(targetPosition, 0.5f);

            yield return new WaitForSeconds(delay);
        }
        _isGameRunning = true;
    }
    public void OnCardFlipped(Card card)
    {
        if (_firstCard == null)
        {
            _firstCard = card;
        }
        else
        {
            _secondCard = card;
            StartCoroutine(JudgeCard());
        }
    }
    IEnumerator JudgeCard()
    {
        _isCardChecking = true;
        yield return new WaitForSeconds(0.75f);
        if (_firstCard._frontSprite == _secondCard._frontSprite)
        {
            _firstCard = null;
            _secondCard = null;
            _matchedCard++;
            if (_matchedCard >= 4)
            {
                EndGame(true);
            }
        }
        else
        {
            _firstCard.UnflipCard();
            _secondCard.UnflipCard();
            _firstCard = null;
            _secondCard = null;
        }
        _isCardChecking = false;
    }

    public void EndGame(bool isWin)
    {
        _isGameRunning = false;
        if (isWin)
        {
            //reward = (int)_gameTime * 100;
            //var varManager = Engine.GetService<ICustomVariableManager>();
            //varManager.TrySetVariableValue("reward", reward);
            //GameManager.userData.Money += reward;
            GameManager.Nani.PlayNani("카드뒤집기", "성공");
        }
        else
        {
            GameManager.Nani.PlayNani("카드뒤집기", "실패");
        }
        mainCam.depth = -1;
    }

    public void ShowSettingUI()
    {
        GameManager.UI.ShowSettingUI();
    }

    public void BackToShop()
    {
        SceneManager.LoadScene("Shop");
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        CreateCards();

        mainCam.depth = 1;

    }
    private void Update()
    {
        if (_isGameRunning)
        {
            _gameTime -= Time.deltaTime;
            timerText.text = $"{Mathf.CeilToInt(_gameTime)}";

            if (_gameTime <= 0)
            {
                EndGame(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Naninovel;

public class CardFlipManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardSpriteList;

    public Transform cardSet;   //카드 덱 위치
    public Transform cardParent; // 카드가 옮겨질 부모 오브젝트

    public TextMeshProUGUI buttonText;
    public Button startButton;

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
            GameObject cardObject = Instantiate(cardPrefab, cardSet.position, Quaternion.identity, cardSet);
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
    
    Vector3 GetCardPosition(int index)
    {
        int colSize = 4;
        int row = index / colSize;
        int col = index % colSize;

        RectTransform cardRect = cardPrefab.GetComponent<RectTransform>();
        RectTransform cardParentRect = cardParent.GetComponent<RectTransform>();

        Vector3 startPos = cardParent.position - new Vector3(cardParentRect.rect.width / 2, cardParentRect.rect.height / 2, 0);
        Vector3 cardSize = new Vector3(cardRect.rect.width, cardRect.rect.height, 0);
        Vector3 spacing = new Vector3(20, 20, 0);

        Vector3 targetPos = startPos + new Vector3(col * (cardSize.x + spacing.x), -row * (cardSize.y + spacing.y), 0);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(targetPos);
        worldPos.z = 0;

        return worldPos;
    }
    IEnumerator MoveCards()
    {
        float delay = 0.2f;
        for (int i = 0; i < _cardList.Count; i++)
        {
            RectTransform cardRect = _cardList[i].GetComponent<RectTransform>();
            Vector3 targetPosition = GetCardPosition(i);

            _cardList[i].transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutQuad);

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

    public async void CardScriptStart()
    {

        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = true;

        await GameManager.Nani.ScriptPlayer.PreloadAndPlayAsync(_cardEnding.ToString());
        GameManager.Nani.InputManager.ProcessInput = true;
    }

    public void EndGame(bool isWin)
    {
        _isGameRunning = false;
        if (isWin)
        {
            _cardEnding = CardFlipEnding.카드뒤집기_성공;
        }
        else
        {
            _cardEnding = CardFlipEnding.카드뒤집기_실패;
        }
        mainCam.depth = -1;
        CardScriptStart();
    }

    public void ShowSettingUI()
    {
        GameManager.UI.ShowSettingUI();
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

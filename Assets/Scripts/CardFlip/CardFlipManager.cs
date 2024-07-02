using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardFlipManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameStartButton;
    [SerializeField]
    private List<GameObject> _cardObjectList;
    [SerializeField]
    private GameObject _cardPrefab;
    [SerializeField]
    private GameObject _prefabParent;
    private GameObject _raycastObj;
    private GameObject _selectedCard1;
    private GameObject _selectedCard2;

    [SerializeField]
    private List<Vector3> _cardPositionList;

    [SerializeField]
    private Sprite _cardFrontSprite1;
    [SerializeField]
    private Sprite _cardFrontSprite2;
    [SerializeField]
    private Sprite _cardFrontSprite3;
    [SerializeField]
    private Sprite _cardFrontSprite4;
    [SerializeField]
    private Sprite _cardBackSprite;

    [SerializeField]
    private int _limitTime;
    [SerializeField]
    private int _numOfCard;     //만들어지는 카드 개수
    [SerializeField]
    private int _numOfFoundCard;

    private float _curTime;
    [SerializeField]
    private bool _canFlipCard;

    [SerializeField]
    private TextMeshProUGUI _timeText;

    private Coroutine _cor1;
    private Coroutine _cor2;
    private Coroutine _cor3;
    private void CastRay()
    {
        //Raycast로 Object 선택
        _raycastObj = null;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider != null)
        {
            _raycastObj = hit.collider.gameObject;
            CardSelect(_raycastObj);
        }
    }
    private void CardPrefabInstantiate(int code)
    {
        //카드 프리팹 생성
        GameObject prefab = Instantiate(_cardPrefab, _prefabParent.transform);
        prefab.gameObject.GetComponent<CardBase>().SetCardCode(code);  //카드 코드 설정
        prefab.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = _cardBackSprite;   //카드 뒷면 설정
        //카드 코드에 따라 앞면 설정
        switch(code)
        {
            case 1:
                prefab.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = _cardFrontSprite1;
                break;
            case 2:
                prefab.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = _cardFrontSprite2;
                break;
            case 3:
                prefab.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = _cardFrontSprite3;
                break;
            case 4:
                prefab.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = _cardFrontSprite4;
                break;
            default:
                break;
        }

        _cardObjectList.Add(prefab);//카드 리스트에 추가

    }
    private void InitPrefabInstantiate()
    {
        // 1 2 3 4를 두개씩 리스트에 Add
        List<int> list = new List<int>();
        for(int i = 1; i <= 4; i++)
        {
            list.Add(i);
            list.Add(i);
        }
        while(list.Count != 0)  //리스트가 비었으면  반복문 종료
        {
            //0~리스트 크기의 랜덤 정수 생성
            int random = Random.Range(0, list.Count);
            CardPrefabInstantiate(list[random]);    //랜덤 정수번째 있는 코드가 해당하는 카드 프리팹 생성
            list.RemoveAt(random);  // 사용한 랜덤 정수번째 원소 삭제
        }
    }
    // 카드 선택하는 함수
    private void BackToFront1(GameObject card)
    {
        _canFlipCard = false;

        card.transform.DORotate(new Vector3(0, 180, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear).onComplete();
    }
    private void JudegeCard()
    {

    }
    IEnumerator BackToFront(GameObject cardObject)
    {
        _canFlipCard = false;
        //카드 선택할때
        var tween = cardObject.transform.DORotate(new Vector3(0, 180, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear);//setease public으로 빼기
        yield return tween.WaitForCompletion();
        //2번씩 실행됨 왜?
        if (_selectedCard2 !=  null)
        {
            //두번째 카드 선택 코루틴 종료 이후 결과 판별
            Debug.Log("두번째 카드 뒤집기 끝");
            CardFlipResult();
        }
        else
        {
            // 카드 하나만 선택했을 경우 코루틴이 끝나고 카드 뒤집기 가능
            Debug.Log("첫번째 카드 뒤집기 끝 카드 선택 가능");
            _canFlipCard = true;
        }
    }
    IEnumerator FrontToBack(GameObject cardObject)
    { 
        //카드 틀렸을때 다시 뒤집기
        var tween = cardObject.transform.DORotate(new Vector3(0, 360, 0), 1.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        yield return tween.WaitForCompletion();
        Debug.Log("카드  선택 가능");
        _canFlipCard = true;
    }
    IEnumerator CardMove()
    {
        // 게임 시작 버튼 누르면 카드 움직임
        for (int i = 0; i < _cardObjectList.Count; i++)
        {
            var tween = _cardObjectList[i].transform.DOLocalMove(_cardPositionList[i], 0.5f);
            yield return tween.WaitForCompletion();
        }
        //카드 다 옮긴 후에 타이머 시작
        StartCoroutine(StartTimer());
        _canFlipCard = true;
    }


    private void CardSelect(GameObject cardObject)
    {
        
        _cor1 = StartCoroutine(BackToFront(cardObject));
        if (_selectedCard1 == null)
        {
            //1번 선택카드가 NULL인 경우 1번에 할당
            _selectedCard1 = cardObject;
            StartCoroutine(BackToFront(cardObject));
            //선택한 카드 선택 불가능하게 설정
            _selectedCard1.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            //1번 선택카드가 NULL이 아니면 2번에 할당
            _selectedCard2 = cardObject;
            StartCoroutine(BackToFront(cardObject));
            //선택한 카드 선택 불가능하게 설정
            _selectedCard2.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    private void CardFlipResult()
    {
        //1번 카드와 2번 카드 코드 비교
        if (_selectedCard1.GetComponent<CardBase>().GetCardCode() == _selectedCard2.GetComponent<CardBase>().GetCardCode())
        {
            Debug.Log("같은  카드네요");
            //동일하면 그대로 두고 찾은 카드수 +2
            _numOfFoundCard += 2;
            //다시 카드 선택가능하게 설정
            _canFlipCard = true;

            //카드 다 찾았으면 게임 종료
            if (_numOfCard ==  _numOfFoundCard)
            {
                _canFlipCard = false;
                GameClear();
            }
        }
        else
        {
            //다르면 뒤집기
            _cor2 = StartCoroutine(FrontToBack(_selectedCard1));
            _cor3 = StartCoroutine(FrontToBack(_selectedCard2));
            _selectedCard1.GetComponent<BoxCollider2D>().enabled = true;
            _selectedCard2.GetComponent<BoxCollider2D>().enabled = true;
        }
        //선택 카드 비우기
        _selectedCard1 = null;
        _selectedCard2 = null;
    }
    private  void GameClear()
    {
        Debug.Log("클리어!");
        //카드 다 찾으면 게임클리어
    }
    public void GameStart()
    {
        StartCoroutine(CardMove());
        _gameStartButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "게임 진행중...";
        _gameStartButton.GetComponent<Button>().interactable = false;

    }
    private void  GameOver()
    {
        //타이머 다 되었을때 게임오버
    }
    IEnumerator StartTimer()
    {
        _curTime = _limitTime;
        _canFlipCard = true;
        while (_curTime  > 0)
        {
            _curTime -= Time.deltaTime;
            _timeText.text = ((int)_curTime).ToString();
            yield return null; 
            if (_numOfCard == _numOfFoundCard)
            {
                yield break;
            }
            if (_curTime <= 0)
            {
                Debug.Log("타이머 종료");
                GameOver();
                _curTime = 0;
                yield break;
            }
        }
    }
    // 만들어야 하는것
    //카드 프리팹 생성 + 같은 카드 이미지
    void Start()
    {
        _canFlipCard = false;
        _numOfFoundCard = 0;
        _timeText.text = _limitTime.ToString();
        InitPrefabInstantiate();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0)&& _canFlipCard)
        {
            CastRay();
        }
    }
}

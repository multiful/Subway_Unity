using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    
    public Sprite _frontSprite;
    public Sprite _backSprite;
    private Image _img;
    private Button _btn;
    private bool _isFlipped = false;

    private CardFlipManager _cardManager;

    // Start is called before the first frame update
    void Start()
    {
        _cardManager = GameObject.Find("CardFlipManager").GetComponent<CardFlipManager>();
        _img = GetComponent<Image>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnCardClick);
        _img.sprite = _backSprite;
    }
    private void OnCardClick()
    {
        if (_isFlipped || _cardManager._isCardChecking) return;
        if (_isFlipped)
        {
            UnflipCard();
        }
        else
        {
            FlipCard();
            _cardManager.OnCardFlipped(this);
        }
    }
    public void FlipCard()
    {
        if (_cardManager._isGameRunning)
        {
            _isFlipped = true;
            transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
            {
                _img.sprite = _frontSprite;
                _img.rectTransform.localScale = new Vector3(-1, 1, 0);
                transform.DORotate(new Vector3(0, 180, 0), 0.25f);
            });
        }

    }
    public void UnflipCard()
    {
        _isFlipped = false;
        transform.DORotate(new Vector3(0, 90, 0), 0.25f).OnComplete(() =>
        {
            _img.sprite = _backSprite;
            transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        });
    }
}

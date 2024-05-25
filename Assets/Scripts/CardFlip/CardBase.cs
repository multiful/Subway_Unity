using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    [SerializeField]//나중에 확인되면 지워도 됨
    private int _cardCode;

    public void SetCardCode(int code)
    {
        _cardCode = code;
    }
    public int GetCardCode()
    {
        return _cardCode;
    }
    // Start is called before the first frame update
    void Start()
    {
        //default
    }
}

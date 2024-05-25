using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField]
    private GameObject _morningSprite;
    [SerializeField]
    private GameObject _eveningSprite;

    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform[] backgrounds;

    private float _leftPosX;
    private float _rightPosX;
    private float _xScreenHalfSize;
    private float _yScreenHalfSize;

    // Start is called before the first frame update
    void Start()
    {
        _leftPosX = 0f;
        _rightPosX = 0f;
        _yScreenHalfSize = Camera.main.orthographicSize;
        _xScreenHalfSize = _yScreenHalfSize * Camera.main.aspect;

        _leftPosX = -(_xScreenHalfSize * 2);
        _rightPosX = _xScreenHalfSize * 2 * backgrounds.Length;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;  i <backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(-speed, 0, 0) * Time.deltaTime;

            if (backgrounds[i].position.x < _leftPosX)
            {
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(nextPos.x + _rightPosX, nextPos.y, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }
}

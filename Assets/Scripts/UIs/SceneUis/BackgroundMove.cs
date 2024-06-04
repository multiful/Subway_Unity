using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private MeshRenderer _render;
    private float _offset;
    [SerializeField]
    private float _speed;
    private void Start()
    {
        _render = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        _offset += Time.deltaTime*_speed;
        _render.material.mainTextureOffset = new Vector2(_offset, 0);
    }
}
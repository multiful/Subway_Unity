using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayRotate : MonoBehaviour
{
    public float rotateSpeed = 200f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.identity;
    }

    void Update()
    {
        rectTransform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);
    }
}

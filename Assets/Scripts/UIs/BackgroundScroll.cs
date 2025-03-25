using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speed = 1000f;
    public RectTransform[] backgrounds;

    float leftPosX = 0f;
    float rightPosX = 0f;
    float xScreenHalfSize;
    float yScreenHalfSize;

    void Start()
    {
        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        leftPosX = -14230f; //-(xScreenHalfSize * 2);
        rightPosX = 19060f; //xScreenHalfSize * 2 * backgrounds.Length;
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].localPosition += new Vector3(-speed, 0, 0) * Time.deltaTime;

            if (backgrounds[i].localPosition.x < leftPosX)
            {
                Vector3 nextPos = backgrounds[i].localPosition;
                nextPos = new Vector3(nextPos.x + rightPosX, nextPos.y, nextPos.z);
                backgrounds[i].localPosition = nextPos;
            }
        }
    }
}
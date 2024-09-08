using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    public Image background;

    public float backgroundSpeed = 0.1f;

    Vector2 backgroundScrollOffset = Vector2.zero;

    private void Update()
    {
        ScrollBackgroundImage();
    }

    private void ScrollBackgroundImage()
    {
        backgroundScrollOffset.x += (backgroundSpeed * Time.deltaTime);
        background.material.mainTextureOffset = backgroundScrollOffset;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
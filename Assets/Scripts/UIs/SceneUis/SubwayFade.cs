using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubwayFade : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    
    [SerializeField]
    private float fadeTime;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.name == "Subway Night")
        {
            DayNightButton(gameObject.name.StartsWith("NightTrigger") ? true : false);
        }
    }

    private IEnumerator DayNight(bool isDaytoNight)
    {
        float t = 0f; //Lerp cycleTime variable
        while (t < fadeTime)
        {
            _image.color = new(1, 1, 1, Mathf.Lerp((isDaytoNight ? 1 : 0), (isDaytoNight ? 0 : 1), t / fadeTime));
            t += Time.deltaTime;
            yield return null;
        }
        t = 0f;
    }

    public void DayNightButton(bool isDaytoNight)
    {
        StartCoroutine(DayNight(isDaytoNight));
    }

}

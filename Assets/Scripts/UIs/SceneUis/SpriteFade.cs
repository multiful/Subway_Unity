using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private float t = 0f; //Lerp time variable

    [SerializeField]
    private float time;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(SubwayFade());
    }

    private IEnumerator SubwayFade()
    {
        while (true)
        {
            DayNightButton(true);
            yield return new WaitForSeconds(time * 0.5f);
            DayNightButton(false);
            yield return new WaitForSeconds(time * 0.5f);
        }
    }

    private IEnumerator DayNight(bool isDaytoNight)
    {
        float end = time / 4;
        while (t < end)
        {
            _sprite.material.color = new(1, 1, 1, Mathf.Lerp((isDaytoNight ? 1 : 0), (isDaytoNight ? 0 : 1), t / end));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private float t = 0f; //Lerp time variable

    [SerializeField]
    private float time = 50f;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(SubwayFade());
    }

    private IEnumerator SubwayFade()
    {
        while (true)
        {
            yield return new WaitForSeconds(time / 8);
            DayNightButton(true);
            yield return new WaitForSeconds(time / 2);
            DayNightButton(false);
            yield return new WaitForSeconds(time / 8 + time / 4);
        }
    }

    private IEnumerator DayNight(bool isDaytoNight)
    {
        float end = time / 8;
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

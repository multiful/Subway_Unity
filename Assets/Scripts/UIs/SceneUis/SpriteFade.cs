using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private float t = 0f; //Lerp cycleTime variable

    [SerializeField]
    private float cycleTime, fadeRatio;
    [SerializeField] private float time1, time2, time3;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(SubwayFade());
    }

    private IEnumerator SubwayFade()
    {
        while (true)
        {
            yield return new WaitForSeconds(cycleTime * time1);
            DayNightButton(true);
            yield return new WaitForSeconds(cycleTime * time2);
            DayNightButton(false);
            yield return new WaitForSeconds(cycleTime * time3);
        }
    }

    private IEnumerator DayNight(bool isDaytoNight)
    {
        float end = cycleTime / fadeRatio;
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

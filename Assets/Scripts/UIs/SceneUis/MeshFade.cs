using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFade : MonoBehaviour
{
    private MeshRenderer _render;
    private float t = 0f; //Lerp time variable

    [SerializeField]
    private float time = 25f;

    private void Start()
    {
        _render = GetComponent<MeshRenderer>();
        StartCoroutine(BackgroundFade());
    }

    private IEnumerator BackgroundFade()
    {
        while (true)
        {
            DayNightButton(true);
            yield return new WaitForSeconds(time);

            DayNightButton(false);
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator DayNight(bool isDaytoNight)
    {
        while (t < time)
        {
            _render.material.color = new(1, 1, 1, Mathf.Lerp((isDaytoNight ? 1 : 0), (isDaytoNight ? 0 : 1), t / time));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshFade : MonoBehaviour
{
    private MeshRenderer _render;
    private float t = 0f; //Lerp time variable

    [SerializeField]
    private float time = 2f;

    private void Start()
    {
        _render = GetComponent<MeshRenderer>();
    }

    private IEnumerator DayNight(bool isDaytoNight)
    {
        while (t < 1f)
        {
            _render.material.color = new(1, 1, 1, Mathf.Lerp((isDaytoNight ? 1 : 0), (isDaytoNight ? 0 : 1), t));
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

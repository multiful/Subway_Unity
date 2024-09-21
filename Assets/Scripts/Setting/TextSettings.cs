using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSettings : MonoBehaviour
{
    public Button touchModeButton;
    public Button autoModeButton;
    public Slider textSpeedSlider;
    public TMP_Text textExample;
    private Coroutine textCoroutine;
    private string exampleText = "This is a text speed example.";


    private void Start()
    {
        // 초기 설정
        SetTextMode(PlayerPrefs.GetInt("TextMode", 0)); // 0: Touch, 1: Auto
        textSpeedSlider.value = PlayerPrefs.GetFloat("TextSpeed", 100.0f);

        // 버튼 클릭 이벤트 설정
        touchModeButton.onClick.AddListener(() => SetTextMode(0));
        autoModeButton.onClick.AddListener(() => SetTextMode(1));

        // 슬라이더 값 변경 시 이벤트 설정
        textSpeedSlider.onValueChanged.AddListener(SetTextSpeed);

        // 텍스트 예시 초기화
        textCoroutine = StartCoroutine(ShowTextExample());
    }

    private void SetTextMode(int mode)
    {
        // 모드 저장
        PlayerPrefs.SetInt("TextMode", mode);

        // 버튼 스타일 변경
        touchModeButton.GetComponentInChildren<TMP_Text>().fontStyle = mode == 0 ? FontStyles.Bold : FontStyles.Normal;
        autoModeButton.GetComponentInChildren<TMP_Text>().fontStyle = mode == 1 ? FontStyles.Bold : FontStyles.Normal;
    }

    private void SetTextSpeed(float speed)
    {
        PlayerPrefs.SetFloat("TextSpeed", speed);
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
        }
        textCoroutine = StartCoroutine(ShowTextExample());
    }

    private IEnumerator ShowTextExample()
    {
        textExample.text = "";

        float delay = 0.1f / Mathf.Max(textSpeedSlider.value, 0.1f); // 최소 딜레이 값 설정

        foreach (char c in exampleText)
        {
            textExample.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}

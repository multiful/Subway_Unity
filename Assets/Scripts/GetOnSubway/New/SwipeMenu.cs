using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwapeMenu : MonoBehaviour
{
    public GameObject scrollbar;
    public Button leftButton;
    public Button rightButton;

    private float scroll_pos = 0;
    private float[] pos;
    private int currentIndex = 0;

    // 아이템 크기 및 위치
    private const float itemWidth = 900f;
    private const float itemSpacing = 300f;
    private const float itemTotalWidth = itemWidth + itemSpacing;
    

    void Start()
    {
        InitializePositions();
        SetInitialScrollPosition();

        // 첫 번째 아이템 크기 조정
        UpdateItemScales();

        // 버튼 클릭 이벤트 설정
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
    }

    void Update()
    {
        HandleMouseScroll();

        // 아이템 크기 조정
        UpdateItemScales();
    }

    void InitializePositions()
    {
        pos = new float[transform.childCount];
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = i * itemTotalWidth;
        }
    }

    void SetInitialScrollPosition()
    {
        currentIndex = 0;
        scroll_pos = pos[currentIndex];
        scrollbar.GetComponent<Scrollbar>().value = 0;
    }

    void HandleMouseScroll()
    {
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value * (pos.Length - 1) * itemTotalWidth;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (Mathf.Abs(scroll_pos - pos[i]) < itemTotalWidth / 2)
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, (float)i / (pos.Length - 1), 0.1f);
                }
            }
        }
    }

    void UpdateItemScales()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (Mathf.Abs(scroll_pos - pos[i]) < itemTotalWidth / 2)
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }

    void MoveLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            scroll_pos = pos[currentIndex];
            StartCoroutine(SmoothScroll((float)currentIndex / (pos.Length - 1)));
        }
    }

    void MoveRight()
    {
        if (currentIndex < pos.Length - 1)
        {
            currentIndex++;
            scroll_pos = pos[currentIndex];
            StartCoroutine(SmoothScroll((float)currentIndex / (pos.Length - 1)));
        }
    }

    IEnumerator SmoothScroll(float targetValue)
    {
        float currentValue = scrollbar.GetComponent<Scrollbar>().value;

        for (float t = 0; t < 1; t += Time.deltaTime * 2) // 0.5초 동안 스무스하게 이동
        {
            scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(currentValue, targetValue, t);
            yield return null;
        }

        scrollbar.GetComponent<Scrollbar>().value = targetValue;
    }
}

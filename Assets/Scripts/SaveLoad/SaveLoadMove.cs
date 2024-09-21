using UnityEngine;
using UnityEngine.UI;

public class SaveLoadUIManager : MonoBehaviour
{
    public GameObject[] saveSlots; // 저장 슬롯 배열
    public Button nextButton, prevButton; // 페이지 넘기는 버튼
    public Text pageIndicator; // 페이지 번호를 표시하는 Text UI
    private int currentPage = 1; // 현재 페이지
    private int totalPages = 4; // 총 페이지 수

    void Start()
    {
        UpdatePageIndicator(); // 페이지 인디케이터 초기화
        UpdateUI(); // 초기 UI 업데이트

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);

        // 페이지 넘김 버튼 활성화/비활성화 상태 초기화
        UpdateButtonStates();
    }

    void UpdatePageIndicator()
    {
        // 페이지 인디케이터를 "1 2 3 4" 형식으로 표시, 현재 페이지는 색깔을 바꿔서 강조 가능
        string pageDisplay = "";
        for (int i = 1; i <= totalPages; i++)
        {
            if (i == currentPage)
            {
                // 현재 페이지는 색상이나 스타일로 강조
                pageDisplay += $"<b><color=yellow>{i}</color></b> ";
            }
            else
            {
                pageDisplay += $"{i} ";
            }
        }

        pageIndicator.text = pageDisplay;
    }

    void UpdateUI()
    {
        // 모든 슬롯 비활성화
        foreach (var slot in saveSlots)
            slot.SetActive(false);

        // 현재 페이지에 해당하는 슬롯만 활성화
        int startSlot = (currentPage - 1) * 4;
        int endSlot = Mathf.Min(startSlot + 4, saveSlots.Length);
        for (int i = startSlot; i < endSlot; i++)
        {
            saveSlots[i].SetActive(true);
        }

        // 페이지 인디케이터 및 버튼 상태 업데이트
        UpdatePageIndicator();
        UpdateButtonStates();
    }

    void NextPage()
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            UpdateUI(); // 페이지 넘김 후 UI 업데이트
        }
    }

    void PreviousPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            UpdateUI(); // 페이지 넘김 후 UI 업데이트
        }
    }

    // 페이지 넘김 버튼 활성화/비활성화 상태를 업데이트하는 함수
    void UpdateButtonStates()
    {
        // 다음 버튼은 현재 페이지가 최대 페이지일 때 비활성화
        nextButton.interactable = currentPage < totalPages;
        // 이전 버튼은 현재 페이지가 첫 페이지일 때 비활성화
        prevButton.interactable = currentPage > 1;
    }
}

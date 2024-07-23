using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ConfirmationPopupMenu : Menu
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public void ActivateMenu(string displayText, UnityAction confirmAction, UnityAction cancelAction)
    {
        // 메뉴 활성화
        this.gameObject.SetActive(true);

        // 텍스트 설정
        this.displayText.text = displayText;

        // 이전 리스너 제거
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        // 리스너 추가
        confirmButton.onClick.AddListener(() => {
            DeactivateMenu();
            confirmAction();
        });
        cancelButton.onClick.AddListener(() => {
            DeactivateMenu();
            cancelAction();
        });
    }


    // 팝업 메뉴 비활성화
    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}

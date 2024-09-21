using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ChoiceLikeability : MonoBehaviour
{
    public TextMeshProUGUI likeabilityText;
    // Start is called before the first frame update
    void Start()
    {
        // 데이터에서 가져오던지 아니면 커스텀 변수에서 가져오던지 하면 될듯
        int likeability = 50;
        likeabilityText.text = $"{likeability}";
    }
}

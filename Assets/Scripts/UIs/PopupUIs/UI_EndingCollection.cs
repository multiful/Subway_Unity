using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_EndingCollection : UI_Popup
{
    enum Buttons
    {
        CloseButton
    }
    enum Texts
    {
        ProgressText,
        LikeabilityText
    }
    enum GameObjects
    {
        Ending1,
        Ending2,
        Ending3,
        Ending4
    }
    enum Images
    {
        BackGround,
        ProgressCircle,
        LikeabilityImage
    }

    // 임시로 적용한 수치
    private int _progress = 75;
    private int _likeability = 60;
    private int[] _ending = { 1 };

    private void Start()
    {
        Init();
        EndingSetup();
    }
    public override void Init()
    {
        //세이브데이터에서 진행도, 호감도, 엔딩 수집 현황 받아옴
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        Button CloseBtn = Get<Button>((int)Buttons.CloseButton);
        Debug.Log(CloseBtn.gameObject.name);
        CloseBtn.onClick.AddListener(Close);
        CloseBtn.onClick.AddListener(() => Debug.Log("adf"));
    }
    public void EndingSetup()
    {
        // 진행도 UI
        Image progressImage = Get<Image>((int)Images.ProgressCircle);
        TextMeshProUGUI progressText = Get<TextMeshProUGUI>((int)Texts.ProgressText);
        progressImage.fillAmount = 0;
        StartCoroutine(FillImage(progressImage));
        progressText.text = _progress.ToString() + "%";

        // 호감도 UI
        TextMeshProUGUI likeabilityText = Get<TextMeshProUGUI>((int)Texts.LikeabilityText);
        likeabilityText.text = _likeability.ToString();
        // 엔딩 모음 UI
        foreach (int end in _ending)
        {
            switch (end)
            {
                case 1:
                    //EndingUnlock();
                    break;
            }
        }
    }
    public IEnumerator FillImage(Image img)
    {
        while (img.fillAmount < _progress / 100)
        {
            img.fillAmount += Time.smoothDeltaTime * 1.5f;
            yield return null;
        }
    }
    private void EndingUnlock(GameObject endingObj, int index)
    {
        endingObj.GetComponent<Button>().interactable = true;
        endingObj.transform.GetChild(0).gameObject.SetActive(false);
        endingObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = index + "번째 엔딩";
    }
}

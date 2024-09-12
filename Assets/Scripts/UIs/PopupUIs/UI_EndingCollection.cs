using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class UI_EndingCollection : UI_Popup
{
    enum Buttons
    {
        Back,
        Setting
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
        ProgressCircle,
        LikeabilityImage
    }

    // 임시로 적용한 수치
    private int _progress;
    private int _likeability = 60;
    private bool[] _ending;

    public GameObject[] endingObjects;
    public Sprite[] unlockEndingSprite;


    private void Start()
    {
        Init();
        EndingSetup();
    }
    public override void Init()
    {
        //세이브데이터에서 진행도, 호감도, 엔딩 수집 현황 받아옴
        GameManager.userData.IsEndingUnlock[0] = true;

        _ending = GameManager.userData.IsEndingUnlock;
        Debug.Log(_ending);
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        Button close = Get<Button>((int)Buttons.Back);
        close.onClick.AddListener(Close);

        Button setting = Get<Button>((int)Buttons.Setting);
        //setting.onClick.AddListener(GameManager.UI.ShowSettingUI);
    }

    public void EndingSetup()
    {
        // 진행도 UI
        _progress = _ending.Length * 25;
        Image progressImage = Get<Image>((int)Images.ProgressCircle);
        TextMeshProUGUI progressText = Get<TextMeshProUGUI>((int)Texts.ProgressText);
        progressImage.fillAmount = 0;


        StartCoroutine(FillImage(progressImage));
        progressText.text = _progress.ToString() + "%";

        // 호감도 UI
        TextMeshProUGUI likeabilityText = Get<TextMeshProUGUI>((int)Texts.LikeabilityText);
        likeabilityText.text = _likeability.ToString();

        // 엔딩 버튼 기능 넣기
        for (int i = 0; i < endingObjects.Length; i++)
        {
            int index = i;
            endingObjects[i].GetComponent<Button>().onClick.AddListener(() => ShowEndingPlayUI(index));
        }

        // 엔딩 모음 UI
        for (int end = 0; end  < _ending.Length; end++)
        {
            if (_ending[end])
            {
                EndingUnlock(endingObjects[end], end);
            }

            //switch (_ending[end])
            //{
            //    case 1:
            //        EndingUnlock(endingObjects[0], 0);
            //        break;
            //    case 2:
            //        EndingUnlock(endingObjects[1], 1);
            //        break;
            //    case 3:
            //        EndingUnlock(endingObjects[2], 2);
            //        break;
            //    case 4:
            //        EndingUnlock(endingObjects[3], 3);
            //        break;
            //}
        }    
    }
    public IEnumerator FillImage(Image img)
    {
        float targetFillAmount = _progress / 100f;

        while (img.fillAmount < targetFillAmount)
        {
            // Time.smoothDeltaTime을 곱해 부드럽게 증가하도록 설정
            img.fillAmount += Time.smoothDeltaTime * 1.5f;

            // 목표치에 도달한 경우, 코루틴을 종료합니다.
            if (img.fillAmount >= targetFillAmount)
            {
                img.fillAmount = targetFillAmount;
                break;
            }

            yield return null;
        }
    }
    private void EndingUnlock(GameObject endingObj, int index)
    {
        endingObj.GetComponent<Image>().sprite = unlockEndingSprite[index];
        endingObj.GetComponent<Button>().interactable = true;
        endingObj.transform.GetChild(0).gameObject.SetActive(false);
    }
    private void ShowEndingPlayUI(int index)
    {
        
        GameManager.UI.ShowEndingPlayUI(index);
    }
}

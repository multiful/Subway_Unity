using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EndingCollectionManager : MonoBehaviour
{
    //[SerializeField]
    //List<int> _endingList;

    //[SerializeField]
    //private float _progress;
    //[SerializeField]
    //private int _likeability;

    //[SerializeField]
    //private GameObject _endingObject;
    //[SerializeField]
    //private GameObject _endingUI;
    //[SerializeField]
    //private GameObject _progressObject;
    //[SerializeField]
    //private GameObject _ending1;
    //[SerializeField]
    //private GameObject _ending2;
    //[SerializeField]
    //private GameObject _ending3;
    //[SerializeField]
    //private GameObject _ending4;


    //public Button test;
    //private void Init()
    //{
    //    _endingObject = GameObject.Find("EndingCollection");
    //    _endingUI = _endingObject.transform.Find("EndingUI").gameObject;
    //    _progressObject = _endingUI.transform.Find("ProgressObject").gameObject;
    //    _ending1 = _endingUI.transform.Find("Ending").gameObject.transform.GetChild(0).gameObject;
    //    _ending2 = _endingUI.transform.Find("Ending").gameObject.transform.GetChild(1).gameObject;
    //    _ending3 = _endingUI.transform.Find("Ending").gameObject.transform.GetChild(2).gameObject;
    //    _ending4 = _endingUI.transform.Find("Ending").gameObject.transform.GetChild(3).gameObject;
    //}
    //void ReadEnding(int index)
    //{
    //    //index번 엔딩 스크립트 진행
    //}
    //// 나중에 UIManager로 기능 이동시켜야함
    //public void PopupOpen()
    //{
    //    _endingUI.SetActive(true);
    //    EndingSetup();
    //}
    //public void PopupClose()
    //{
    //    _endingUI.SetActive(false);
    //}
    //public void EndingPlay(int index)
    //{
    //    Debug.Log(index + "번째 엔딩 플레이");
    //}
    //private void EndingUnlock(GameObject endingObj, int index)
    //{
    //    endingObj.GetComponent<Button>().interactable = true;
    //    endingObj.transform.GetChild(0).gameObject.SetActive(false);
    //    endingObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = index + "번째 엔딩";
    //}
    //public void EndingSetup()
    //{
    //    Image _progressImage;
    //    _progressImage = _progressObject.transform.Find("ProgressCircle").GetComponent<Image>();
    //    _progressImage.fillAmount = 0;
    //    StartCoroutine(FillImage(_progressImage));
    //    _progressObject.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>().text = _progress.ToString() + "%";

    //    for (int i = 0; i < _endingList.Count; i++)
    //    {
    //        int endingNum = _endingList[i];
    //        switch (endingNum)
    //        {
    //            case 1:
    //                EndingUnlock(_ending1, 1);
    //                break;
    //            case 2:
    //                EndingUnlock(_ending2, 2);
    //                break;
    //            case 3:
    //                EndingUnlock(_ending3, 3);
    //                break;
    //            case 4:
    //                EndingUnlock(_ending4, 4);
    //                break;
    //        }
    //    }
    //}
    //public IEnumerator FillImage(Image img)
    //{
    //    while(img.fillAmount < _progress/100)
    //    {
    //        img.fillAmount += Time.smoothDeltaTime * 1.5f;
    //        yield return null;
    //    }
    //}
    //private void Awake()
    //{
    //    Init();
    //}
    //// Start is called before the first frame update
    //void Start()
    //{
    //    //나중에 세이브데이터에서 받아오면 됨
    //    //_ending = SaveLoadManager.Inst.userData._ending;
    //    //_likeability = SaveLoadManager.Inst.userData
    //}

    public void ShowEndingCollection()
    {
        GameManager.UI.ShowPopupUI<UI_EndingCollection>();
    }
}

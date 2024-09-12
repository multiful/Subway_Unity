using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class EndingCollectionManager : MonoBehaviour
{
    public Button backButton;
    public Button settingButton;

    public TextMeshProUGUI progressText;
    public TextMeshProUGUI likeabilityText;

    public GameObject[] endings;

    public Image progressImg;
    public Image likeabilityImg;

    public Sprite[] unlockedEndingSprites;

    private int _progress;
    private int _likeability;
    private bool[] _openedEnding;

    private void Init()
    {
        //일단 1번 엔딩만 열어둔 상태
        GameManager.userData.IsEndingUnlock[0] = true;

        _openedEnding = GameManager.userData.IsEndingUnlock;
        //_openedEnding = new bool[4];
        //_openedEnding[0] = true;
        //_openedEnding[3] = true;

    }
    private void EndingSetup()
    {
        //_progress = 진행도 가져옴
        _progress = 22; //임시 설정 값
        progressImg.fillAmount = 0;

        StartCoroutine(Fillimg(progressImg));
        progressText.text = _progress.ToString() + "%";

        likeabilityText.text = _likeability.ToString();

        for (int end = 0; end < _openedEnding.Length; end++)
        {
            if (_openedEnding[end])
            {
                EndingUnlock(endings[end], end);
            }
        }
    }
    private IEnumerator Fillimg(Image img)
    {
        float targetFillAmount = _progress / 100f;
        while(img.fillAmount < targetFillAmount)
        {
            img.fillAmount += Time.smoothDeltaTime * 1.5f;

            if (img.fillAmount >= targetFillAmount)
            {
                img.fillAmount = targetFillAmount;
                break;
            }
            yield return null;
        }
    }
    private void EndingUnlock(GameObject ending, int index)
    {
        ending.GetComponent<Image>().sprite = unlockedEndingSprites[index];
        ending.GetComponent<Button>().interactable = true;
        ending.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void ShowEndingPlayUI(int index)
    {
        GameManager.UI.ShowEndingPlayUI(index);
    }
    public void BackButtonFunc()
    {
        // 씬 로드
        SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        EndingSetup();
    }
}

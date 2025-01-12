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

    public GameObject[] endings;

    public Image progressImg;

    public Sprite[] unlockedEndingSprites;

    private bool[] _openedEnding;


    //public EndingSO[] _endingSOList;
    //private Ending[] _endingList;
    private void Init()
    {
        _openedEnding = GameManager.userData.IsEndingUnlock;

    }
    private void EndingSetup()
    {

        for (int end = 0; end < _openedEnding.Length; end++)
        {
            if (_openedEnding[end])
            {
                EndingUnlock(endings[end], end);
            }
        }
    }
    private void EndingUnlock(GameObject ending, int index)
    {
        ending.GetComponent<Image>().sprite = unlockedEndingSprites[index];
        ending.GetComponent<Button>().interactable = true;
        ending.transform.GetChild(0).gameObject.SetActive(false);
    }
    //private void GetEndingData()
    //{

    //}
    public void ShowEndingPlayUI(EndingSO so)
    {
        GameManager.UI.ShowEndingPlayUI(so);
    }
    public void BackButtonFunc()
    {
        SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
        EndingSetup();
    }
}

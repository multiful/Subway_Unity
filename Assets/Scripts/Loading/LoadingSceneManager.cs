using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    public static float loadingTime;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName, float time = 2f)
    {
        nextScene = sceneName;
        loadingTime = time;
        SceneManager.LoadScene("Loading");
    }

    private IEnumerator LoadScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (timer > loadingTime)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}

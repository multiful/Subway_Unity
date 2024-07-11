using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T s_instance;
    public static T Inst { get { init(); return s_instance; } }

    static void init()
    {
        GameObject go = GameObject.Find(typeof(T).Name);

        if (go == null)
        {
            go = new GameObject(typeof(T).Name);
            go.AddComponent<T>();
        }

        if (go.transform.parent != null && go.transform.root != null)
            DontDestroyOnLoad(go.transform.root.gameObject);
        else
            DontDestroyOnLoad(go);

        s_instance = go.GetComponent<T>();
    }

    void Awake()
    {
        init();
        if (s_instance != this)
        {
            Destroy(gameObject.GetComponent<T>());
        }
    }

}
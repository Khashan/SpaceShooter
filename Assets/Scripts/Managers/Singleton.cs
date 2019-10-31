using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_Instance;
    public static T Instance
    {
        get {return s_Instance;}
    }

    protected virtual void Awake()
    {
        if(s_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            s_Instance = GetComponent<T>();
        }

        DontDestroyOnLoad(gameObject);
    }
}
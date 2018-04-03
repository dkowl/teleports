using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (FindObjectsOfType<T>().Length > 1)
                {
                    Debug.LogWarningFormat("Something is wrong. There is more than one singleton of {0}", typeof(T).ToString());
                }

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("(Singleton) " + typeof(T).ToString());
                    instance = singletonObject.AddComponent<T>();

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

}

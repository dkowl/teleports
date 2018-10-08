using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingletonInstance
{
    void OnFirstAccess();
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<T>();

            if (FindObjectsOfType<T>().Length > 1)
            {
                Debug.LogWarningFormat("Something is wrong. There is more than one singleton of {0}", typeof(T).ToString());
            }

            if (instance != null)
            {
                InstanceFirstAccess();
                return instance;
            }

            GameObject singletonObject = new GameObject("(Singleton) " + typeof(T).ToString());
            instance = singletonObject.AddComponent<T>();

            InstanceFirstAccess();
            return instance;
        }
    }

    private static void InstanceFirstAccess()
    {
        var singletonInstance = instance as ISingletonInstance;
        singletonInstance?.OnFirstAccess();
    }
}

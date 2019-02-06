using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface ISingletonInstance
{
    void OnFirstAccess();
}

[AttributeUsage(AttributeTargets.Class)]
public class DisallowGameObjectCreation : System.Attribute
{

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

            if (instance == null && AllowGameObjectCreation())
            {
                GameObject singletonObject = new GameObject("(Singleton) " + typeof(T).ToString());
                instance = singletonObject.AddComponent<T>();
            }

            if (instance != null)
            {
                InstanceFirstAccess();
            }
            else
            {
                Debug.LogError($"Instance of {typeof(T).FullName} cannot be initialized");
            }

            return instance;
        }
    }

    private static void InstanceFirstAccess()
    {
        var singletonInstance = instance as ISingletonInstance;
        singletonInstance?.OnFirstAccess();
    }

    private static bool AllowGameObjectCreation()
    {
        var typeInfo = typeof(T).GetTypeInfo();
        var attribute = typeInfo.GetCustomAttribute<DisallowGameObjectCreation>();
        return attribute == null;
    }
}

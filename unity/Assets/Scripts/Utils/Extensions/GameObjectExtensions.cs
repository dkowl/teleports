using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Teleports.Utils;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class GameObjectExtensions {

    public static void MakeVisible(this GameObject gameObject)
    {
        gameObject.transform.localScale = Vector3.one;
    }

    public static void MakeInvisible(this GameObject gameObject)
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    public static T GetComponentInChildrenNamed<T>(this GameObject gameObject, string name) where T : Component
    {
        Transform foundTransform = gameObject.transform.FindRecursive(name);
        if (foundTransform != null)
        {
            return foundTransform.gameObject.GetComponent<T>();
        }
        else return null;
    }

    public static List<GameObject> GetChildren(this GameObject gameObject)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(gameObject.transform);
        var result = new List<GameObject>();
        while (queue.Count > 0)
        {
            Transform currentTransform = queue.Dequeue();
            for (int i = 0; i < currentTransform.childCount; i++)
            {
                queue.Enqueue(currentTransform.GetChild(i));
            }
            result.Add(currentTransform.gameObject);
        }
        return result;
    }

    public static void SetLayerIncludingChildren(this GameObject gameObject, int layer)
    {
        foreach (var gObject in gameObject.GetChildren())
        {
            gObject.layer = layer;
        }
    }

    public static void InitComponent<T>(this GameObject gameObject, ref T component) where T : Component
    {
        if (component == null)
        {
            component = gameObject.GetComponent<T>();
        }

        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
    }

    public static T CopyValuesFrom<T>(this Component component, T other) where T : Component
    {
        Type componentType = component.GetType();
        if (componentType != other.GetType()) return null;

        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                    BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] propertyInfos = componentType.GetProperties(bindingFlags);
        foreach (var propertyInfo in propertyInfos)
        {
            if (!propertyInfo.CanWrite) continue;

            try
            {
                propertyInfo.SetValue(component, propertyInfo.GetValue(other));
            }
            catch
            {
                Debug.Assert(false, "Exception when copying component property");
            }
        }

        FieldInfo[] fieldInfos = componentType.GetFields(bindingFlags);
        foreach (var fieldInfo in fieldInfos)
        {
            fieldInfo.SetValue(component, fieldInfo.GetValue(other));
        }

        return component as T;
    }

    public static T AddCopyOfComponent<T>(this GameObject gameObject, T component) where T : Component
    {
        return gameObject.AddComponent<T>().CopyValuesFrom(component);
    }

#if UNITY_EDITOR
    public static void CreateFromEditor(this GameObject gameObject, UnityEditor.MenuCommand command)
    {
        GameObjectUtility.SetParentAndAlign(gameObject, command.context as GameObject);
        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeObject = gameObject;
    }
#endif
}

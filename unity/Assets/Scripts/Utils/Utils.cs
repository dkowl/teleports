﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl.Runtime;
using UnityEngine;

namespace Teleports.Utils
{
    public static class Utils
    {
        public static float baseAspect = 16f / 9f;

        public static bool Approximately(Vector3 a, Vector3 b)
        {
            return
                Mathf.Approximately(a.x, b.x) &&
                Mathf.Approximately(a.y, b.y) &&
                Mathf.Approximately(a.z, b.z);
        }

        public static int EnumCount(System.Type type)
        {
            return System.Enum.GetValues(type).Length;
        }

        public static bool HasParameter(this Animator animator, string name)
        {
            //if (!animator.gameObject.activeInHierarchy) return false;

            foreach(var parameter in animator.parameters)
            {
                if(parameter.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static Transform FindRecursive(this Transform transform, string name)
        {
            Transform result = transform.Find(name);
            if (result != null) return result;
            else
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    result = transform.GetChild(i).Find(name);
                    if (result != null) return result;
                }
            }
            return null;
        }

        public static List<T> GetComponentsInObjects<T>(IList<GameObject> gameObjects) where T : Component
        {
            var result = new List<T>();
            foreach(var gameObject in gameObjects)
            {
                T component = gameObject.GetComponent<T>();
                if(component != null)
                {
                    result.Add(component);
                }
            }
            return result;
        }

        public static T GetComponentInChildrenNamed<T>(this Component component, string name) where T : Component
        {
            return component.gameObject.GetComponentInChildrenNamed<T>(name);
        }

        public static void FindOrSpawnChildWithComponent<T>(this Component parentComponent, ref T component, string name, bool restrictFindChildrenWithName = false) where T : Component
        {
            if(component == null)
            {
                if (restrictFindChildrenWithName)
                {
                    component = parentComponent.GetComponentInChildrenNamed<T>(name);
                }
                else
                {
                    component = parentComponent.GetComponentInChildren<T>();
                }
                
                if(component == null)
                {
                    GameObject spawnedObject = new GameObject(name);
                    spawnedObject.transform.parent = parentComponent.transform;
                    component = spawnedObject.AddComponent<T>();
                }
            }
        }

        public static void Extend<T>(this List<T> list, int targetCount, T defaultValue)
        {
            if(list == null)
            {
                list = new List<T>();
            }

            if(list.Capacity < targetCount)
            {
                list.Capacity = targetCount;
            }

            if (list.Count < targetCount)
            {
                for(int i = 0; i<targetCount - list.Count; i++)
                {
                    list.Add(defaultValue);
                }
            }
        }

        public static void InitWithValues<T>(ref List<T> list, int targetCount, T defaultValue)
        {
            list = new List<T>(targetCount);

            for(int i = 0; i< targetCount; i++)
            {
                list.Add(defaultValue);
            }
        }

        public static void InitWithNew<T>(ref List<T> list, int targetCount) where T : new()
        {
            list = new List<T>(targetCount);

            for (int i = 0; i < targetCount; i++)
            {
                list.Add(new T());
            }
        }

        public static T RandomElement<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
    }

    public static class ListUtils
    {
        public static List<T> DeepCopy<T>(this List<T> from) where T : class, IDeepCopyable
        {
            var to = new List<T>(from.Count);
            foreach (var elem in from)
            {
                to.Add(elem.DeepCopy() as T);
            }
            return to;
        }
    }

    public static class GameObjectUtils
    {
        public static List<T> GetComponentsInSiblings<T>(this Component component)
        {
            var result = new List<T>();
            foreach (var sibling in component.transform.GetSiblings())
            {
                T foundComponent = sibling.gameObject.GetComponent<T>();
                if(foundComponent != null) result.Add(foundComponent);
            }
            return result;
        }

        public static List<Transform> GetSiblings(this Transform transform)
        {
            var result = new List<Transform>();
            if (transform.parent == null) return result;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (i == transform.GetSiblingIndex()) continue;
                result.Add(transform.parent.GetChild(i));
            }
            return result;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T result = gameObject.GetComponent<T>();
            if (result == null) result = gameObject.AddComponent<T>();
            Debug.Assert(result != null);
            return result;
        }
    }

    public static class RomanNumbers
    {
        private static string result;
        private static int x;

        private static bool CheckAndAppend(string letter, int number)
        {
            if (x >= number)
            {
                result += letter;
                x -= number;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string RomanNumber(int number)
        {
            result = "";
            x = number;

            while (CheckAndAppend("M", 1000)) ;
            CheckAndAppend("CM", 900);
            CheckAndAppend("D", 500);
            CheckAndAppend("CD", 400);
            while (CheckAndAppend("C", 100)) ;
            CheckAndAppend("XC", 90);
            CheckAndAppend("L", 50);
            CheckAndAppend("XL", 40);
            while (CheckAndAppend("X", 10)) ;
            CheckAndAppend("IX", 9);
            CheckAndAppend("V", 5);
            CheckAndAppend("IV", 4);
            while (CheckAndAppend("I", 1)) ;

            return result;
        }
    }
}
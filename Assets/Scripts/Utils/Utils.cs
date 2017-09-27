using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Teleports.Utils
{
    public static class Utils
    {
        public static float baseAspect = 16f / 9f;

        public static void makeVisible(this GameObject gameObject)
        {
            gameObject.transform.localScale = Vector3.one;
        }

        public static void makeInvisible(this GameObject gameObject)
        {
            gameObject.transform.localScale = Vector3.zero;
        }

        public static bool Approximately(Vector3 a, Vector3 b)
        {
            return
                Mathf.Approximately(a.x, b.x) &&
                Mathf.Approximately(a.y, b.y) &&
                Mathf.Approximately(a.z, b.z);
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
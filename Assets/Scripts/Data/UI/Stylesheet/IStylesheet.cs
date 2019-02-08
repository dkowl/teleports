using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStylesheet
{
    T GetValue<T>(string typeKey, string key);
    IList<string> GetKeys(string typeKey);
}

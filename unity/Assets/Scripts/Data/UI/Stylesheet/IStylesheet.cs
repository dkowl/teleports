using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStylesheet
{
    T GetValue<T>(StylesheetKey key);
    IList<string> GetKeys(string typeKey);
}

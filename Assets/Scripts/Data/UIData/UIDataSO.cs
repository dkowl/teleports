using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UI Data", menuName = "Data/UI/UI")]
public class UIDataSO : ScriptableObjectDataWrapper<UIData>
{
#if UNITY_EDITOR
    [Button]
    public void AddAllMenus()
    {
        AddScriptableObjectWrappedData<MenuData, MenuDataSO>(data.MenusConcrete);
    }
#endif
}

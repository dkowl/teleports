using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{

    void SwitchScene(string sceneName);
    void SwitchSceneThenInvoke(string sceneName, System.Action postloadAction);
    string CurrentSceneName { get; }
}

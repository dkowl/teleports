using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneTransitionHandler  {

    public static void HandleSceneTransition(string fromName, string toName)
    {
        if(fromName == SceneNames.Main && toName == SceneNames.Home)
        {
            //MainData.SaveSO.Load();
            Debug.LogWarning("saving/loading GameState not implemented");
        }
        else if (fromName == SceneNames.World && toName == SceneNames.Main)
        {
            //MainData.SaveSO.Save();
            Debug.LogWarning("saving/loading GameState not implemented");
        }
    }
}

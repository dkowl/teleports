using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Main class
//Singleton
//Manages async Scene loading
//Acts as loading screen
public class MainController : MonoBehaviour {

    //singleton instance
    private static MainController mainController;
    
    //private variables
    private string currentSceneName = "Main";
    private string nextSceneName;
    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count };
    private SceneState sceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] updateDelegates;

    //inspector variables
    public string startSceneName = "Start";
    public LoadingGraphics loadingGraphics;

    //public static methods
    public static void SwitchScene(string nextSceneName)
    {
        if(mainController!= null)
        {
            if(mainController.currentSceneName != nextSceneName)
            {
                mainController.nextSceneName = nextSceneName;
            }
        }
    }

    //unity event methods
    void Awake()
    {
        //Preserve gameObject
        DontDestroyOnLoad(gameObject);

        //Assign singleton instance
        mainController = this;

        //Setup deletate array
        updateDelegates = new UpdateDelegate[(int)SceneState.Count];

        //Populate delegate array
        updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
        updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
        updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
        updateDelegates[(int)SceneState.Run] = UpdateSceneRun;

        nextSceneName = startSceneName;
        sceneState = SceneState.Run;
    }

    void OnDestroy()
    {
        //Clean up delegates
        if(updateDelegates != null)
        {
            for(int i = 0; i < updateDelegates.Length; i++)
            {
                updateDelegates[i] = null;
            }
            updateDelegates = null;
        }

        //Clean up singleton instance
        if(mainController != null)
        {
            mainController = null;
        }
    }
    
    void Update()
    {
        if (updateDelegates[(int)sceneState] != null)
        {
            updateDelegates[(int)sceneState]();
        }
    }

    //private methods
    private void UpdateSceneReset()
    {
        //Run GC
        System.GC.Collect();
        sceneState = SceneState.Preload;
    }

    //Start displaying loading screen, assign load task
    private void UpdateScenePreload()
    {
        loadingGraphics.SetActive(true);
        sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName);
        sceneState = SceneState.Load;
    }

    //Loading and updating progress
    private void UpdateSceneLoad()
    {
        if (sceneLoadTask == null || sceneLoadTask.isDone)
        {
            sceneState = SceneState.Unload;
        }
        else
        {
            loadingGraphics.UpdateProgress(sceneLoadTask.progress);
        }
    }

    //Unloading unused resources
    private void UpdateSceneUnload()
    {
        if(resourceUnloadTask == null)
        {
            resourceUnloadTask = Resources.UnloadUnusedAssets();
        }
        else
        {
            if (resourceUnloadTask.isDone)
            {
                resourceUnloadTask = null;
                sceneState = SceneState.Postload;
            }
        }
    }

    //Hide loading screen, set current scene
    private void UpdateScenePostload()
    {
        loadingGraphics.SetActive(false);
        currentSceneName = nextSceneName;
        sceneState = SceneState.Ready;
    }

    //Stuff to do before running
    private void UpdateSceneReady()
    {
        sceneState = SceneState.Run;
    }

    //Keep checking if nextScene was changed
    private void UpdateSceneRun()
    {
        if(currentSceneName != nextSceneName)
        {
            sceneState = SceneState.Reset;
        }
    }

}

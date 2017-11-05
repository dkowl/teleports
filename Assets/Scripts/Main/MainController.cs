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
        if(updateDelegates != null)
        {
            for(int i = 0; i < updateDelegates.Length; i++)
            {
                updateDelegates[i] = null;
            }
            updateDelegates = null;
        }
        
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

    void OnApplicationQuit()
    {
        MainData.SaveDataSO.Save();
    }

    //private methods
    private void UpdateSceneReset()
    {
        System.GC.Collect();
        sceneState = SceneState.Preload;
    }
    
    private void UpdateScenePreload()
    {
        loadingGraphics.SetActive(true);
        sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName);

        if(currentSceneName == "Main" && nextSceneName == "Start")
        {
            MainData.SaveDataSO.Load();
        }
        else if(currentSceneName == "World" && nextSceneName == "Start")
        {
            MainData.SaveDataSO.Save();
        }
        sceneState = SceneState.Load;
    }
    
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
    
    private void UpdateScenePostload()
    {
        loadingGraphics.SetActive(false);
        currentSceneName = nextSceneName;
        sceneState = SceneState.Ready;
    }
    
    private void UpdateSceneReady()
    {
        sceneState = SceneState.Run;
    }
    
    private void UpdateSceneRun()
    {
        if(currentSceneName != nextSceneName)
        {
            sceneState = SceneState.Reset;
        }
    }

}

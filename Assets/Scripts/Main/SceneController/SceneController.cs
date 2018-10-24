using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Manages async Scene loading
//Acts as loading screen
public class SceneController : MonoBehaviour, ISceneController {
    
    private string currentSceneName = SceneNames.Main;
    private string nextSceneName;
    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count };
    private SceneState sceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] updateDelegates;
    private ILoadingGraphics loadingGraphics;
    private System.Action postloadAction;

    [SerializeField] private string startSceneName = SceneNames.Home;

    public void SwitchScene(string sceneName)
    {
        if (currentSceneName != sceneName)
        {
            nextSceneName = sceneName;
        }
    }

    public void SwitchSceneThenInvoke(string sceneName, System.Action postloadAction)
    {
        if (currentSceneName == sceneName) return;

        nextSceneName = sceneName;
        this.postloadAction = postloadAction;
    }

    private void Awake()
    {
        updateDelegates = new UpdateDelegate[(int)SceneState.Count];
        
        updateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
        updateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        updateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        updateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        updateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
        updateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
        updateDelegates[(int)SceneState.Run] = UpdateSceneRun;

        nextSceneName = startSceneName;
        sceneState = SceneState.Run;

        loadingGraphics = Main.LoadingGraphics;
    }

    private void Update()
    {
        if (updateDelegates[(int)sceneState] != null)
        {
            updateDelegates[(int)sceneState]();
        }
    }

    private void OnDestroy()
    {
        if (updateDelegates == null) return;

        for (int i = 0; i < updateDelegates.Length; i++)
        {
            updateDelegates[i] = null;
        }
        updateDelegates = null;
    }
    
    private void UpdateSceneReset()
    {
        System.GC.Collect();
        sceneState = SceneState.Preload;
    }
    
    private void UpdateScenePreload()
    {
        loadingGraphics?.SetActive(true);
        sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName);
        SceneTransitionHandler.HandleSceneTransition(currentSceneName, nextSceneName);
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
            loadingGraphics?.UpdateProgress(sceneLoadTask.progress);
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
        if (postloadAction != null)
        {
            postloadAction.Invoke();
            postloadAction = null;
        }
        loadingGraphics?.SetActive(false);
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

    public string CurrentSceneName => currentSceneName;

}

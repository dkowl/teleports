using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class Launcher
{
    private static List<string> unloadedScenePaths;

    private static readonly string unloadedScenePathsCountKey = "unloadedScenePathsCount";
    private static readonly string unloadedScenePathKeyPrefix = "unloadedScenePath";

    static Launcher()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    [MenuItem("Teleports/Play %p")]
    private static void Play()
    {
        UnloadExtraScenes();
        EditorApplication.isPlaying = true;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange stateChange)
    {
        if (stateChange == PlayModeStateChange.ExitingEditMode)
        {
            UnloadExtraScenes();
        }
        else if (stateChange == PlayModeStateChange.EnteredEditMode)
        {
            LoadUnloadedScenes();
        }
    }

    private static void UnloadExtraScenes()
    {
        unloadedScenePaths = new List<string>();
        for (int sceneIndex = 0; sceneIndex < EditorSceneManager.sceneCount; sceneIndex++)
        {
            var currentScene = EditorSceneManager.GetSceneAt(sceneIndex);
            if (currentScene.isLoaded && currentScene.name != "Main")
            {
                Debug.Log($"Unloading extra scene {currentScene.name}");
                EditorSceneManager.CloseScene(currentScene, false);
                unloadedScenePaths.Add(currentScene.path);
            }
        }
        
        SerializeUnloadedScenePaths();
    }

    private static void LoadUnloadedScenes()
    {
        DeserializeUnloadedScenePaths();
        foreach (var path in unloadedScenePaths)
        {
            Debug.Log($"Loading back scene at {path}");
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }
    }

    private static void SerializeUnloadedScenePaths()
    {
        EditorPrefs.SetInt(unloadedScenePathsCountKey, unloadedScenePaths.Count);
        for (int i = 0; i < unloadedScenePaths.Count; i++)
        {
            EditorPrefs.SetString(unloadedScenePathKeyPrefix + i, unloadedScenePaths[i]);
        }
    }

    private static void DeserializeUnloadedScenePaths()
    {
        unloadedScenePaths = new List<string>();
        for (int i = 0; i < EditorPrefs.GetInt(unloadedScenePathsCountKey); i++)
        {
            unloadedScenePaths.Add(EditorPrefs.GetString(unloadedScenePathKeyPrefix + i));
        }
    }

}

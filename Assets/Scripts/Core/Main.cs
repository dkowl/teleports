using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class Main : Singleton<Main>
{
    [SerializeField] private LoadingGraphics loadingGraphics;

    private IMessageBus messageBus;
    private ISceneController sceneController;

    private void Awake()
    {
        messageBus = new MessageBus();
        InitializeInterfaceWithComponent<SceneController, ISceneController>(out sceneController);
    }

    private void InitializeInterfaceWithComponent<T, I>(out I i) where T : Component, I
    {
        T temp = null;
        gameObject.InitComponent(ref temp);
        i = temp;
    }

    public static IMessageBus MessageBus => Instance.messageBus;
    public static ISceneController SceneController => Instance.sceneController;
    public static ILoadingGraphics LoadingGraphics => Instance.loadingGraphics;

}

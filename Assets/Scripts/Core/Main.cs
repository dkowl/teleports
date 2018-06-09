using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class Main : Singleton<Main>
{
    [SerializeField] private LoadingGraphics loadingGraphicsConcrete;
    [SerializeField] private Persistence persistenceConcrete;

    private IMessageBus messageBus;
    private ISceneController sceneController;
    private ILoadingGraphics loadingGraphics;
    private IPersistence persistence;

    private IStaticData staticData;

    private void Awake()
    {
        messageBus = new MessageBus();

        InitializeInterfaceWithComponent<SceneController, ISceneController>(out sceneController);
        loadingGraphics = loadingGraphicsConcrete;

        persistence = persistenceConcrete;

        staticData = persistence.GetStaticData();
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
    public static IStaticData StaticData => Instance.staticData;

}

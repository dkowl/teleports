using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private IMessageBus messageBus;
    private SceneController sceneController;

    private void Awake()
    {
        messageBus = new MessageBus();
        sceneController = new SceneController();
    }



    public IMessageBus MessageBus => messageBus;

}

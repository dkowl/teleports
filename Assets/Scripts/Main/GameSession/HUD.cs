using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HUD - Head-Up Display
public class HUD {

    private GameObject mainCanvas;
    private bool endScreenOn;
    private GameObject endScreen;
    private GameObject hudGameObject;

    public void Spawn(GameObject parentObject)
    {
        mainCanvas = Main.UISystem.SpawnCanvas("HUD");
        hudGameObject = Object.Instantiate(Resources.Load("UI/World/HUD"), mainCanvas.transform) as GameObject;
    }

    public void Despawn()
    {
        Object.Destroy(mainCanvas);
    }

    public void EndScreen(string text)
    {
        if (endScreenOn) return;

        endScreen = Object.Instantiate(Resources.Load("Prefabs/UI/EndScreen"), mainCanvas.transform) as GameObject;
        endScreen.GetComponent<EndScreenUI>().SetText(text);
        endScreenOn = true;
    }
}

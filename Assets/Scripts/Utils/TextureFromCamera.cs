using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TextureFromCamera : MonoBehaviour {

    public Texture2D texture;
    public Camera cam;

    [Button]
    public void GenerateTexture()
    {
        int
            width = 128,
            height = 128;

        RenderTexture previousTargetRT = cam.targetTexture;
        RenderTexture myRT = new RenderTexture(width, height, 16);
        cam.targetTexture = myRT;
        cam.Render();
        cam.targetTexture = previousTargetRT;

        RenderTexture previousActiveRT = RenderTexture.active;
        RenderTexture.active = myRT;
        texture = new Texture2D(width, height);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();
        RenderTexture.active = previousActiveRT;
    }
}

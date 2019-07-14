using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TextureArrayFromModels : MonoBehaviour {

    public int width;
    public int height;
    public TextureFormat textureFormat;
    public bool mipmap;
    public Texture2DArray textureArray;
    public MeshFilter[] meshFilters;
    public CameraMeshTargeter cameraTargeter;
    
    [Button]
    public void GenerateArray()
    {
        textureArray = new Texture2DArray(width, height, meshFilters.Length, textureFormat, mipmap);

        Camera cam = cameraTargeter.Camera;
        MeshFilter previousTargetMeshFilter = cameraTargeter.TargetMeshFilter;
        RenderTexture previousTargetRT = cam.targetTexture;
        RenderTexture myRT = new RenderTexture(width, height, 16);
        cam.targetTexture = myRT;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            cameraTargeter.SetTarget(meshFilters[i]);
            cam.Render();

            Graphics.CopyTexture(myRT, 0, 0, 0, 0, width, height, textureArray, i, 0, 0, 0);
        }

        cam.targetTexture = previousTargetRT;
        cameraTargeter.SetTarget(previousTargetMeshFilter);
    }
}

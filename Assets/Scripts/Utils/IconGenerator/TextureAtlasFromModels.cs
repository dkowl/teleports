using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TextureAtlasFromModels : MonoBehaviour {

    public int width = 128;
    public int height = 128;
    public TextureFormat textureFormat = TextureFormat.ARGB32;
    public bool mipmap = true;
    public int padding = 1;
    public int maximumAtlasSize = 1024;
    public Texture2D atlas;
    public Rect[] atlasUvs;
    public Texture2D[] textures;
    public MeshFilter[] meshFilters;
    public CameraMeshTargeter cameraTargeter;

    [Button]
    public void GenerateAtlas()
    {
        textures = new Texture2D[meshFilters.Length];
        Camera cam = cameraTargeter.Camera;
        MeshFilter previousTargetMeshFilter = cameraTargeter.TargetMeshFilter;
        RenderTexture previousActiveRT = RenderTexture.active;
        RenderTexture previousTargetRT = cam.targetTexture;
        RenderTexture myRT = new RenderTexture(width, height, 16);
        RenderTexture.active = myRT;
        cam.targetTexture = myRT;



        for (int i = 0; i < meshFilters.Length; i++)
        {
            cameraTargeter.SetTarget(meshFilters[i]);
            cam.Render();
            textures[i] = new Texture2D(width, height);
            textures[i].ReadPixels(new Rect(0, 0, width, height), 0, 0);
            textures[i].Apply();
        }

        cam.targetTexture = previousTargetRT;
        RenderTexture.active = previousActiveRT;
        cameraTargeter.SetTarget(previousTargetMeshFilter);

        atlas = new Texture2D(16, 16);
        atlasUvs = atlas.PackTextures(textures, padding, maximumAtlasSize);
    }
}

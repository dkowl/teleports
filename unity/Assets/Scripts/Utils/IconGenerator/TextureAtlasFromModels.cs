using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class TextureAtlasFromModels {

    [SerializeField] private int width = 128;
    [SerializeField] private int height = 128;
    [SerializeField] private TextureFormat textureFormat = TextureFormat.ARGB32;
    [SerializeField] private bool mipmap = true;
    [SerializeField] private int padding = 4;
    [SerializeField] private int maximumAtlasSize = 1024;

    [ShowInInspector] private MeshFilter[] meshFilters;
    [ShowInInspector] private CameraMeshTargeter cameraTargeter;

    [ShowInInspector, ReadOnly] private Texture2D atlas;
    [ShowInInspector, ReadOnly] private Rect[] atlasUvs;
    [ShowInInspector, ReadOnly] private int textureCount;
    private Texture2D[] textures;

    public TextureAtlasFromModels(MeshFilter[] meshFilters, CameraMeshTargeter cameraTargeter)
    {
        this.meshFilters = meshFilters;
        this.cameraTargeter = cameraTargeter;

        GenerateAtlas();
    }

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
            textures[i] = new Texture2D(width, height, textureFormat, mipmap);
            textures[i].ReadPixels(new Rect(0, 0, width, height), 0, 0);
            textures[i].Apply();
        }

        cam.targetTexture = previousTargetRT;
        RenderTexture.active = previousActiveRT;
        cameraTargeter.SetTarget(previousTargetMeshFilter);

        atlas = new Texture2D(16, 16);
        textureCount = textures.Length;
        atlasUvs = atlas.PackTextures(textures, padding, maximumAtlasSize);
    }

    public Rect GetUv(int id)
    {
        if (id >= 0 && id < textureCount)
        {
            return atlasUvs[id];
        }
        else
            return Rect.zero;
    }

    public Texture2D Atlas
    {
        get { return atlas; }
    }

    public Rect[] Uvs
    {
        get { return atlasUvs; }
    }

    public int TextureCount
    {
        get { return textureCount; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teleports.Utils;

public abstract class PortraitUI : MonoBehaviour {

    private RectTransform rectTransform;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Camera cam;
    [SerializeField] private CameraMeshTargeter camMeshTargeter;
    [SerializeField] private GameObject camObject;
    [SerializeField] private CameraMeshTargeter.MeshComponentType objectType;
    [SerializeField] protected MeshFilter meshFilter;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;

    protected abstract CameraMeshTargeter.MeshComponentType SpawnModel();

    private void OnEnable()
    {
        gameObject.InitComponent(ref rectTransform);
        InitRenderTexture();
        InitRawTexture();
        objectType = SpawnModel();
        InitCamera();
    }

    private void Start()
    {
        InitCameraTargeter();
    }

    private void InitRenderTexture()
    {
        int width = Mathf.NextPowerOfTwo((int)rectTransform.rect.width);
        int height = Mathf.NextPowerOfTwo((int)rectTransform.rect.height);
        renderTexture = new RenderTexture(width, height, 24);
    }

    private void InitRawTexture()
    {
        gameObject.InitComponent(ref rawImage);
        rawImage.texture = renderTexture;
    }

    private void InitCamera()
    {
        if (camObject == null)
        {
            camObject = new GameObject("PortraitCamera");
        }
        if (cam == null)
        {
            cam = camObject.AddComponent<Camera>();            
        }
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.targetTexture = renderTexture;
    }

    protected void InitCameraTargeter()
    {
        if (camMeshTargeter == null)
        {
            camMeshTargeter = camObject.AddComponent<CameraMeshTargeter>();
        }
        switch (objectType) {
            case CameraMeshTargeter.MeshComponentType.SkinnedMeshRenderer:
                camMeshTargeter.SetTarget(skinnedMeshRenderer);
                break;
            case CameraMeshTargeter.MeshComponentType.MeshFilter:
                camMeshTargeter.SetTarget(meshFilter);
                break;
            default:
                return;
        }
    }

    public enum ObjectType
    {
        MeshFilter,
        SkinnedMeshRenderer
    }
}

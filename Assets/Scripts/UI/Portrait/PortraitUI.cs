using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Teleports.Utils;

public class PortraitUI : MonoBehaviour {

    private RectTransform rectTransform;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private RawImage rawImage;
    [SerializeField] private ObjectType objectType;
    [SerializeField] private UnitModelSpawner unitModelSpawner;
    [SerializeField] private Camera cam;
    [SerializeField] private CameraMeshTargeter camMeshTargeter;
    [SerializeField] private GameObject camObject;


    private void OnEnable()
    {
        gameObject.InitComponent(ref rectTransform);
        InitRenderTexture();
        InitRawTexture();
        SpawnModel();
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

    private void SpawnModel()
    {
        switch (objectType)
        {
            case ObjectType.InventoryCurrentUnit:
                InventoryMenu parentMenu = GetComponentInParent<InventoryMenu>();
                Debug.Assert(parentMenu != null);
                gameObject.InitComponent(ref unitModelSpawner);
                unitModelSpawner.AddSpawnData();
                unitModelSpawner.SetPositionOffset(SpecialSpawnPlaces.InventoryPlayer);
                unitModelSpawner.SetRotationOffset(new Vector3(0, 180, 0));
                unitModelSpawner.UnitData = parentMenu.UnitData;
                break;
        }
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

    private void InitCameraTargeter()
    {
        if (camMeshTargeter == null)
        {
            camMeshTargeter = camObject.AddComponent<CameraMeshTargeter>();
        }
        camMeshTargeter.SetTarget(unitModelSpawner.SkinnedMeshRenderer);
    }

    public enum ObjectType
    {
        InventoryCurrentUnit
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraMeshTargeter : MonoBehaviour {

    [SerializeField] private MeshComponentType meshComponentType;
    [SerializeField, OnValueChanged("UpdateMesh")] private MeshFilter meshFilter;
    [SerializeField, OnValueChanged("UpdateMesh")] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Camera cam;
    [SerializeField] private float paddingPercentage = 0.25f;
    [SerializeField] private float distance = 10.0f;

    private Mesh mesh;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (meshFilter != null)
        {
            SetTarget(meshFilter);
        }
        else if(skinnedMeshRenderer != null)
        {
            SetTarget(skinnedMeshRenderer);
        }
    }

    private void Update()
    {
        if(MeshComponent != null) Target();
    }

    public void SetTarget(MeshFilter newTarget)
    {
        if (newTarget == null) return;
        meshComponentType = MeshComponentType.MeshFilter;
        meshFilter = newTarget;
        UpdateMesh();
        Target();
    }

    public void SetTarget(SkinnedMeshRenderer newTarget)
    {
        if (newTarget == null) return;
        meshComponentType = MeshComponentType.SkinnedMeshRenderer;
        skinnedMeshRenderer = newTarget;
        UpdateMesh();
        Target();
    }

    private void Target()
    {
        List<Vector3> cornerPoints = GetAllCornerPoints(mesh.bounds, MeshComponent.transform.position);
        Rect boundingViewportRect = GetBoundingViewportRect(cornerPoints, cam);
        CenterCameraOnViewportRect(cam, boundingViewportRect, paddingPercentage);
        SetDistanceFromPoint(cam, mesh.bounds.center, distance);
    }

    private void UpdateMesh()
    {
        switch (meshComponentType)
        {
            case MeshComponentType.MeshFilter:
                mesh = meshFilter.sharedMesh;
                break;
            case MeshComponentType.SkinnedMeshRenderer:
                mesh = skinnedMeshRenderer.sharedMesh;
                break;
        }
    }

    private static List<Vector3> GetAllCornerPoints(Bounds bounds, Vector3 position)
    {

        var result = new List<Vector3>();

        var axisMultiplierValues = new byte[8, 3]
        {
            {0, 0, 0 },
            {0, 0, 1 },
            {0, 1, 0 },
            {0, 1, 1 },
            {1, 0, 0 },
            {1, 0, 1 },
            {1, 1, 0 },
            {1, 1, 1 }
        };

        for(int i = 0; i < 8; i++)
        {
            Vector3 size = bounds.size;
            Vector3 vectorToAdd = new Vector3(
                size.x * axisMultiplierValues[i, 0],
                size.y * axisMultiplierValues[i, 1],
                size.z * axisMultiplierValues[i, 2]
                );
            result.Add(position + bounds.min + vectorToAdd);
        }
        return result;
    }

    private static Rect GetBoundingViewportRect(List<Vector3> points, Camera camera)
    {
        float 
            minX = Mathf.Infinity,
            minY = Mathf.Infinity,
            maxX = Mathf.NegativeInfinity, 
            maxY = Mathf.NegativeInfinity;

        foreach(var worldPoint in points)
        {
            Vector3 point = camera.WorldToViewportPoint(worldPoint);

            minX = Mathf.Min(point.x, minX);
            minY = Mathf.Min(point.y, minY);
            maxX = Mathf.Max(point.x, maxX);
            maxY = Mathf.Max(point.y, maxY);
        }

        Vector2 position = new Vector2(minX, minY);
        Vector2 size = new Vector2(maxX - minX, maxY - minY);
        return new Rect(position, size);
    }

    private static void CenterCameraOnViewportPoint(Camera camera, Vector2 targetPoint)
    {
        Vector3 rightOffset = camera.transform.right;
        rightOffset *= camera.orthographicSize * 2 * camera.aspect;
        rightOffset *= targetPoint.x - 0.5f;

        Vector3 upOffset = camera.transform.up;
        upOffset *= camera.orthographicSize * 2;
        upOffset *= targetPoint.y - 0.5f;

        camera.transform.localPosition += rightOffset + upOffset;
    }

    private static void CenterCameraOnViewportRect(Camera camera, Rect rect, float paddingPercentage)
    {
        CenterCameraOnViewportPoint(camera, rect.center);
        camera.orthographicSize *= Mathf.Max(rect.height, rect.width);
        camera.orthographicSize *= 1 / (1 - paddingPercentage);
    }

    private static float DistanceFromPointToPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint)
    {
        planeNormal.Normalize();
        return Vector3.Dot(planeNormal, point - planePoint);
    }

    private static void SetDistanceFromPoint(Camera camera, Vector3 point, float targetDistance)
    {
        Vector3 planeNormal = -camera.transform.forward;
        float currentDistance = DistanceFromPointToPlane(camera.transform.position, planeNormal, point);
        camera.transform.localPosition += (targetDistance - currentDistance) * planeNormal;
    }

    public Camera Camera => cam;
    public MeshFilter TargetMeshFilter => meshFilter;
    private Component MeshComponent
    {
        get
        {
            switch (meshComponentType)
            {
                case MeshComponentType.MeshFilter:
                    return meshFilter;
                case MeshComponentType.SkinnedMeshRenderer:
                    return skinnedMeshRenderer;
                default:
                    return null;
            }
        }
    }

    public enum MeshComponentType
    {
        None,
        MeshFilter,
        SkinnedMeshRenderer
    }
}

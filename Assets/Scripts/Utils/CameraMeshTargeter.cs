using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraMeshTargeter : MonoBehaviour {

    [SerializeField, OnValueChanged("UpdateMesh")] MeshFilter meshFilter;
    [SerializeField] Camera cam;
    [SerializeField] float paddingPercentage = 0.1f;
    [SerializeField] float distance = 10.0f;

    Mesh mesh;

    void Awake()
    {
        cam = GetComponent<Camera>();
        SetTarget(meshFilter);
    }

    void Update()
    {
        Target();
    }

    public void SetTarget(MeshFilter newTarget)
    {
        meshFilter = newTarget;
        UpdateMesh();
        Target();
    }

    void Target()
    {
        List<Vector3> cornerPoints = GetAllCornerPoints(mesh.bounds, meshFilter.transform.position);
        Rect boundingViewportRect = GetBoundingViewportRect(cornerPoints, cam);
        CenterCameraOnViewportRect(cam, boundingViewportRect, paddingPercentage);
        SetDistanceFromPoint(cam, mesh.bounds.center, distance);
    }

    void UpdateMesh()
    {
        mesh = meshFilter.sharedMesh;
    }

    List<Vector3> GetAllCornerPoints(Bounds bounds, Vector3 position)
    {

        List<Vector3> result = new List<Vector3>();

        byte[,] axisMultiplierValues = new byte[8, 3]
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

    Rect GetBoundingViewportRect(List<Vector3> points, Camera camera)
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

    void CenterCameraOnViewportPoint(Camera camera, Vector2 targetPoint)
    {
        Vector3 rightOffset = camera.transform.right;
        rightOffset *= camera.orthographicSize * 2 * camera.aspect;
        rightOffset *= targetPoint.x - 0.5f;

        Vector3 upOffset = camera.transform.up;
        upOffset *= camera.orthographicSize * 2;
        upOffset *= targetPoint.y - 0.5f;

        camera.transform.localPosition += rightOffset + upOffset;
    }

    void CenterCameraOnViewportRect(Camera camera, Rect rect, float paddingPercentage)
    {
        CenterCameraOnViewportPoint(camera, rect.center);
        camera.orthographicSize *= Mathf.Max(rect.height, rect.width);
        camera.orthographicSize *= 1 / (1 - paddingPercentage);
    }

    float DistanceFromPointToPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint)
    {
        planeNormal.Normalize();
        return Vector3.Dot(planeNormal, point - planePoint);
    }

    void SetDistanceFromPoint(Camera camera, Vector3 point, float targetDistance)
    {
        Vector3 planeNormal = -camera.transform.forward;
        float currentDistance = DistanceFromPointToPlane(camera.transform.position, planeNormal, point);
        camera.transform.localPosition += (targetDistance - currentDistance) * planeNormal;
    }

    public Camera Camera
    {
        get { return cam; }
    }

    public MeshFilter TargetMeshFilter
    {
        get { return meshFilter; }
    }
}

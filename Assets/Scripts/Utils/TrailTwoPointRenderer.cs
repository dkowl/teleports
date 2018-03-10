using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TrailTwoPointRenderer : MonoBehaviour
{

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField, MinValue(0)] private float trailTime;
    [SerializeField] private Material material;
    [SerializeField] private Color beginColor;
    [SerializeField] private Color endColor;
    [SerializeField] private float minimalVelocity;

    private List<Snapshot> snapshots;
    private float startTime;

    private GameObject trailObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private float lastHighVelocityTime;

    private struct Snapshot
    {
        public Vector3 aPos;
        public Vector3 bPos;
        public float time;

        public Snapshot(Vector3 aPos, Vector3 bPos, float time)
        {
            this.aPos = aPos;
            this.bPos = bPos;
            this.time = time;
        }

        public Vector3 MidPoint => Vector3.Lerp(aPos, bPos, 0.5f);
    }

    private void Awake()
    {
        snapshots = new List<Snapshot>();
        startTime = Time.time;

        trailObject = new GameObject(name + "_Trail");
        meshFilter = trailObject.AddComponent<MeshFilter>();
        meshRenderer = trailObject.AddComponent<MeshRenderer>();
        if (material != null)
        {
            meshRenderer.sharedMaterial = material;
        }

        Debug.Assert(IsValid(), "Transforms not set");
    }

    private void Update()
    {
        snapshots.Add(new Snapshot(pointA.position, pointB.position, Time.time - startTime));
        UpdateVelocity();
        meshFilter.mesh = IsHighVelocity() ? GenerateMesh() : null;
    }

    private void UpdateVelocity()
    {
        Debug.Log(CurrentVelocity());
        if (CurrentVelocity() > minimalVelocity) lastHighVelocityTime = Time.time - startTime;
    }

    private Mesh GenerateMesh()
    {
        float minTime = snapshots[snapshots.Count - 1].time - trailTime;

        int beginIndex = 0;
        while (beginIndex < snapshots.Count - 1 && snapshots[beginIndex].time < minTime) beginIndex++;
        int validSnapshotCount = snapshots.Count - beginIndex;
        if (validSnapshotCount < 2) return null;

        int vertCount = validSnapshotCount * 2;
        int triCount = (vertCount - 2) * 6;
        var vertices = new Vector3[vertCount];
        var colors = new Color[vertCount];
        var uvs = new Vector2[vertCount];
        var triangles = new int[triCount];
        for (int i = beginIndex; i < snapshots.Count; i++)
        {
            int iRelative = i - beginIndex;
            int iVert = iRelative * 2;
            int iTri = (iRelative - 1) * 6;
            Snapshot curSnapshot = snapshots[i];

            vertices[iVert] = curSnapshot.aPos;
            vertices[iVert + 1] = curSnapshot.bPos;

            float distanceRatio = 1.0f - (float)iRelative / validSnapshotCount;

            Color color = Color.Lerp(beginColor, endColor, distanceRatio);
            colors[iVert] = color;
            colors[iVert + 1] = color;

            uvs[iVert] = new Vector2(distanceRatio, 0);
            uvs[iVert] = new Vector2(distanceRatio, 1);

            if (iTri >= 0)
            {
                triangles[iTri] = iVert - 2;
                triangles[iTri + 1] = iVert + 1;
                triangles[iTri + 2] = iVert;

                triangles[iTri + 3] = iVert - 2;
                triangles[iTri + 4] = iVert - 1;
                triangles[iTri + 5] = iVert + 1;
            }
        }

        Mesh mesh = meshFilter.mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        return mesh;
    }

    private bool IsValid()
    {
        return pointA != null && pointB != null;
    }

    private float Velocity(Snapshot x, Snapshot y, float t)
    {
        float distance = (y.MidPoint - x.MidPoint).magnitude;
        return distance / t;
    }

    private float CurrentVelocity()
    {
        return snapshots.Count >= 2 ? Velocity(snapshots[snapshots.Count - 1], snapshots[snapshots.Count - 2], Time.deltaTime) : 0;
    }

    private bool IsHighVelocity()
    {
        return Time.time - lastHighVelocityTime - startTime < trailTime;
    }
}

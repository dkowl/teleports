using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTriangularizator : MonoBehaviour {

    private GameObject instantiatedObject;
    private GameObject modelObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public static Mesh Triangularize(SkinnedMeshRenderer meshRenderer)
    {
        Mesh mesh = new Mesh();
        meshRenderer.BakeMesh(mesh);
        return Triangularize(mesh);
    }

    void Triangularize(MeshFilter meshFilter)
    {
        meshFilter.mesh = Triangularize(meshFilter.mesh);
    }

	public static Mesh Triangularize(Mesh mesh)    {

        int[] triangles = mesh.triangles;

        int l = triangles.Length;
        int[] newTriangles = new int[l];

        Vector3[] vertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[l];

        Vector2[] uvs = mesh.uv;
        Vector2[] newUvs = new Vector2[l];
        Vector3[] normals = mesh.normals;
        Vector3[] newNormals = new Vector3[l];

        for(int i = 0; i<l; i++)
        {
            newTriangles[i] = i;

            int oldVertIndex = triangles[i];
            newVertices[i] = vertices[oldVertIndex];
            newUvs[i] = uvs[oldVertIndex];
            newNormals[i] = normals[oldVertIndex];
        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.uv = newUvs;
        mesh.normals = newNormals;

        return mesh;
    }

    public void SpawnObject()
    {
        if (instantiatedObject == null)
        {
            FindModelObject();
            SkinnedMeshRenderer modelMeshRenderer = modelObject.GetComponent<SkinnedMeshRenderer>();

            instantiatedObject = new GameObject("TriangularizedMesh");
            instantiatedObject.transform.parent = gameObject.transform;
            instantiatedObject.transform.localPosition = modelObject.transform.localPosition;
            instantiatedObject.transform.localRotation = modelObject.transform.localRotation;

            meshFilter = instantiatedObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = Triangularize(modelMeshRenderer);

            meshRenderer = instantiatedObject.AddComponent<MeshRenderer>();
            meshRenderer.material = modelMeshRenderer.sharedMaterial;
            meshRenderer.material.shader = Shader.Find("Custom/Explode");
        }
    }

    private void FindModelObject()
    {
        if (modelObject == null)
        {
            modelObject = GetComponentInChildren<SkinnedMeshRenderer>().gameObject;
        }
    }
}

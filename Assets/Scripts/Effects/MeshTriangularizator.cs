using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTriangularizator : MonoBehaviour {

    private GameObject instantiatedObject;
    private GameObject modelObject;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public RuntimeAnimatorController animController;

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
        Color[] newColors = new Color[l];
        float randomValue = 0f;

        for(int i = 0; i<l; i++)
        {
            newTriangles[i] = i;

            int oldVertIndex = triangles[i];
            newVertices[i] = vertices[oldVertIndex];
            newUvs[i] = uvs[oldVertIndex];
            //newNormals[i] = normals[oldVertIndex];

            if (i % 3 == 0)
            {
                randomValue = 1 - (newVertices[i].y - mesh.bounds.min.y) / mesh.bounds.size.y;
            }
            newColors[i] = new Color(randomValue, 0, 0);
        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.uv = newUvs;
        //mesh.normals = newNormals;
        mesh.colors = newColors;
        mesh.RecalculateNormals();

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
            instantiatedObject.transform.position = modelObject.transform.position;
            instantiatedObject.transform.rotation = modelObject.transform.rotation;

            meshFilter = instantiatedObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = Triangularize(modelMeshRenderer);

            meshRenderer = instantiatedObject.AddComponent<MeshRenderer>();
            meshRenderer.material = modelMeshRenderer.sharedMaterial;
            meshRenderer.material.shader = Shader.Find("Custom/Explode");
            meshRenderer.material.SetFloat("_FloorHeight", meshRenderer.bounds.min.y);

            if(animController != null)
            {
                Animator anim = instantiatedObject.AddComponent<Animator>();
                anim.runtimeAnimatorController = animController;

                modelObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void Stop()
    {
        if(animController!= null && instantiatedObject != null && modelObject != null)
        {
            Destroy(instantiatedObject);
            modelObject.transform.parent.gameObject.SetActive(true);
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

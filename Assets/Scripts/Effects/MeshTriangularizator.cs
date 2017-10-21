using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshTriangularizator : MonoBehaviour {

    void Awake()
    {
        Triangularize(GetComponent<MeshFilter>());
    }

    void Triangularize(MeshFilter meshFilter)
    {
        meshFilter.mesh = Triangularize(meshFilter.mesh);
    }

	Mesh Triangularize(Mesh mesh)    {

        int[] triangles = mesh.triangles;
        int[] newTriangles = new int[triangles.Length];

        Vector3[] vertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[triangles.Length];

        for(int i = 0; i<triangles.Length; i++)
        {
            newTriangles[i] = i;
            newVertices[i] = vertices[triangles[i]];
        }

        mesh.triangles = newTriangles;
        mesh.vertices = newVertices;

        return mesh;
    }
}

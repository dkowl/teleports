using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGraphics : MonoBehaviour {

    int sizeX_, sizeY_;
    float tileSize_;
    public Material material_;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void generateMesh(TileMap tileMap, Vector3 offset, float tileSize)
    {
        sizeX_ = tileMap.getSizeX();
        sizeY_ = tileMap.getSizeY();
        tileSize_ = tileSize;

        int vertexNum = 4 * sizeX_ * sizeY_;

        Vector3[] vertices = new Vector3[vertexNum];
        Vector2[] uvs = new Vector2[vertexNum];
        Vector3[] normals = new Vector3[vertexNum];
        int[] triangles = new int[6 * sizeX_ * sizeY_];
        for (int x = 0; x < sizeX_; x++)
        {
            for(int y = 0; y<sizeY_; y++)
            {
                int index = (x * sizeX_ + y) * 4;
                int triangleIndex = (x * sizeX_ + y) * 6;
                float
                    xPos = offset.x + x * tileSize,
                    zPos = offset.z + y * tileSize,
                    yPos = 0;
                //making a square
                vertices[index] = new Vector3(xPos, yPos, zPos);
                vertices[index + 1] = new Vector3(xPos + tileSize, yPos, zPos);
                vertices[index + 2] = new Vector3(xPos + tileSize, yPos, zPos + tileSize);
                vertices[index + 3] = new Vector3(xPos, yPos, zPos + tileSize);

                float left = 0, right = 0, bottom = 0, top = 0;
                typeToUvs(tileMap.getType(x, y), ref left, ref right, ref bottom, ref top);
                uvs[index] = new Vector2(left, bottom);
                uvs[index + 1] = new Vector2(right, bottom);
                uvs[index + 2] = new Vector2(right, top);
                uvs[index + 3] = new Vector2(left, top);

                for(int i = 0; i<4; i++)
                {
                    normals[index + i] = Vector3.up;
                }

                triangles[triangleIndex] = index + 2;
                triangles[triangleIndex + 1] = index + 1;
                triangles[triangleIndex + 2] = index;
                triangles[triangleIndex + 3] = index + 3;
                triangles[triangleIndex + 4] = index + 2;
                triangles[triangleIndex + 5] = index;
            }
        }

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshCollider>();

        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.normals = normals;
        mesh.triangles = triangles;

        gameObject.GetComponent<MeshRenderer>().sharedMaterial = material_;

        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void typeToUvs(Tile.Type type, ref float l, ref float r, ref float b, ref float t)
    {
        if(type == Tile.Type.GRASS)
        {
            l = 0.01f; r = 0.49f;
            b = 0.0f; t = 1.0f;
        }
        else if (type == Tile.Type.WATER)
        {
            l = 0.51f; r = 0.99f;
            b = 0.0f; t = 1.0f;
        }
    }
}

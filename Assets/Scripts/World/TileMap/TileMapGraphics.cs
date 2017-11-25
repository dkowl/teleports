using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGraphics : MonoBehaviour {

    [SerializeField] Material material;
    int sizeX, sizeY;

    public void GenerateMesh(TileMap tileMap, Vector3 offset, float tileSize)
    {
        sizeX = tileMap.getSizeX();
        sizeY = tileMap.getSizeY();

        int vertexNum = 4 * sizeX * sizeY;

        Vector3[] vertices = new Vector3[vertexNum];
        Vector2[] uvs = new Vector2[vertexNum];
        Vector3[] normals = new Vector3[vertexNum];
        int[] triangles = new int[6 * sizeX * sizeY];
        for (int x = 0; x < sizeX; x++)
        {
            for(int y = 0; y<sizeY; y++)
            {
                int index = (x * sizeX + y) * 4;
                int triangleIndex = (x * sizeX + y) * 6;
                float
                    xPos = offset.x + x * tileSize,
                    zPos = offset.z + y * tileSize,
                    yPos = 0;
                Vector4 hPts = tileMap.getHeightPoints(x, y);
                //making a square
                vertices[index] = new Vector3(xPos, yPos + hPts.x, zPos);
                vertices[index + 1] = new Vector3(xPos + tileSize, yPos + hPts.z, zPos);
                vertices[index + 2] = new Vector3(xPos + tileSize, yPos + hPts.w, zPos + tileSize);
                vertices[index + 3] = new Vector3(xPos, yPos + hPts.y, zPos + tileSize);

                //setting uvs
                float left = 0, right = 0, bottom = 0, top = 0;
                TypeToUvs(tileMap.getType(x, y), ref left, ref right, ref bottom, ref top);
                uvs[index] = new Vector2(left, bottom);
                uvs[index + 1] = new Vector2(right, bottom);
                uvs[index + 2] = new Vector2(right, top);
                uvs[index + 3] = new Vector2(left, top);

                //setting normals
                for(int i = 0; i<4; i++)
                {
                    normals[index + i] = Vector3.up;
                }

                //setting triangles
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

        gameObject.GetComponent<MeshRenderer>().sharedMaterial = material;

        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void TypeToUvs(Tile.TerrainType type, ref float l, ref float r, ref float b, ref float t)
    {
        float padding = 0.03f;
        if(type == Tile.TerrainType.GRASS)
        {
            l = 0f; r = 0.5f;
            b = 0.5f; t = 1f;
        }
        else if (type == Tile.TerrainType.WATER)
        {
            l = 0f; r = 0.5f;
            b = 0f; t = 0.5f;
        }
        l += padding;
        r -= padding;
        b += padding;
        t -= padding;
    }
}

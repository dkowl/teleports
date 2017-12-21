using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TextureGenerator : MonoBehaviour {

    private static readonly ValueDropdownList<Vector2Int> textureSizeValues = new ValueDropdownList<Vector2Int>
    {
        { "4x4", new Vector2Int(4, 4) }
    };

    [SerializeField, ValueDropdown("textureSizeValues")] private Vector2Int textureSize;
	[SerializeField] private int colorCount = 16;
    [SerializeField] private Color[] colors;
    [SerializeField, ReadOnly] private bool isInitialized = false;
    [SerializeField, ReadOnly] Texture2D texture;

    private void OnEnable()
    {
        if (!isInitialized)
        {
            InitializeColors();
        }
    }

    [Button]
    private void InitializeColors()
    {
        colors = Teleports.ColorUtils.DistinctColors16();
        isInitialized = true;
    }

    [Button]
    private void CreateTexture()
    {
        texture = new Texture2D(textureSize.x, textureSize.y, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Point;

        int colorIndex = 0;
        for(int i = 0; i<textureSize.y; i++)
        {
            for(int j = textureSize.x - 1; j>=0; j--)
            {
                Color color = Color.cyan;
                if (colorIndex < colors.Length) color = colors[colorIndex];
                texture.SetPixel(i, j, color);
                colorIndex++;
            }
        }
        texture.Apply();
    }

    [Button]
    private void ApplyTexture()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if(meshRenderer != null)
        {
            meshRenderer.sharedMaterial.mainTexture = texture;
        }
    }

}

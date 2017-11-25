using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WorldPositionGrid
{
    Vector3 startPosition;
    float offsetX;
    float offsetZ;
    int rows;
    int cols;

    int currentIndex;

    public WorldPositionGrid(Vector3 startPosition, float offset, int size)
    {
        this.startPosition = startPosition;
        offsetX = offset;
        offsetZ = offset;
        rows = size;
        cols = size;
        currentIndex = 0;
    }

    public void Reset()
    {
        currentIndex = 0;
    }

    public Vector3 CurrentPosition
    {
        get
        {
            int currentRow = currentIndex / cols;
            int currentCol = currentIndex % rows;
            return startPosition + new Vector3(offsetX * currentCol, 0, offsetZ * currentRow);
        }
    }

    public Vector3 NextPosition
    {
        get
        {
            currentIndex++;
            currentIndex %= MaxIndex;
            return CurrentPosition;
        }
    }

    int MaxIndex
    {
        get { return rows * cols; }
    }
}
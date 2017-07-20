using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Carousel : MonoBehaviour {

    public RectTransform content_;
    public RectTransform[] children_;

    public bool vertical_, horizontal_;

    //zoomSize - how far from the view center child has to be to be fully scaled down
    //zoomMin - minimal scale of a child
    public float zoomSize_, zoomMin_;

    //position normalized to be compared with children positions
    public Vector2 position_;

    void Update()
    {
        updatePosition();

        updateChildScales();

    }

    //gets content position
    Vector2 position()
    {
        Vector2 anchPos = content_.anchoredPosition;
        anchPos.x *= -1;
        return anchPos;
    }

    void updatePosition()
    {
        position_ = position();
    }

    //sets content position to pos
    void enforcePosition(Vector2 pos)
    {
        Vector2 newPos = pos;
        pos.x *= -1;
        content_.anchoredPosition = pos;
    }

    //returns distance between child and position_
    Vector2 getDelta(RectTransform child)
    {
        Vector2 delta = position_ - child.anchoredPosition;

        if (!horizontal_) delta.x = 0;
        if (!vertical_) delta.y = 0;

        return delta;

    }

    float getChildScale(RectTransform child)
    {
        float deltaMag = getDelta(child).magnitude;

        if(deltaMag > zoomSize_)
        {
            return zoomMin_;
        }
        else
        {
            return Mathf.Lerp(zoomMin_, 1, 1 - (deltaMag / zoomSize_));
        }
    }

    void updateChildScale(RectTransform child)
    {
        Vector3 scaleV = Vector3.one;
        float scale = getChildScale(child);

        scaleV.x = scale;
        scaleV.y = scale;

        child.localScale = scaleV;
    }

    void updateChildScales()
    {
        foreach(RectTransform child in children_)
        {
            updateChildScale(child);
        }
    }

}

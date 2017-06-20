using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class TargetMarker : MonoBehaviour {

    enum Arrow
    {
        Down,
        Right,
        Left,
        Up
    }

    const int arrowNo_ = 4;

    public float minSize_, amplitude_;

    Canvas canvas_;
    Unit targetUnit_;
    Vector3[] baseVector_ = new Vector3[arrowNo_];
    GameObject[] arrows_ = new GameObject[arrowNo_];


    void Awake()
    {
        canvas_ = gameObject.transform.GetChild(0).GetComponent<Canvas>();

        baseVector_[(int)Arrow.Down] = new Vector3(0, -1, 0);
        baseVector_[(int)Arrow.Right] = new Vector3(1, 0, 0);
        baseVector_[(int)Arrow.Up] = new Vector3(0, 1, 0);
        baseVector_[(int)Arrow.Left] = new Vector3(-1, 0, 0);

        for(int i = 0; i< arrowNo_; i++)
        {
            arrows_[i] = gameObject.transform.GetChild(0).GetChild(i).gameObject;
            arrows_[i].GetComponent<OscillatingMovement>().amplitude_ = baseVector_[i] * amplitude_;
        }
    }

    void Update()
    {
        if (targetUnit_ != null) {
            transform.position = targetUnit_.gameObject.transform.position;
            gameObject.makeVisible();
        }
        else
        {
            gameObject.makeInvisible();
        }
    }

    public void setTargetUnit(Unit unit)
    {
        if (unit == targetUnit_) return;

        targetUnit_ = unit;

        if (targetUnit_ != null)
        {
            setSize(unit.Size + minSize_);
        }
    }

    public void setSize(float size)
    {
        for(int i = 0; i< arrowNo_; i++)
        {
            arrows_[i].GetComponent<OscillatingMovement>().begin_ = baseVector_[i] * size;
        }
    }
}

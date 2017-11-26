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

    const int arrowNo = 4;

    public float minSize, amplitude;
    
    Unit targetUnit;
    Vector3[] baseVector = new Vector3[arrowNo];
    GameObject[] arrows = new GameObject[arrowNo];


    void Awake()
    {
        baseVector[(int)Arrow.Down] = new Vector3(0, -1, 0);
        baseVector[(int)Arrow.Right] = new Vector3(1, 0, 0);
        baseVector[(int)Arrow.Up] = new Vector3(0, 1, 0);
        baseVector[(int)Arrow.Left] = new Vector3(-1, 0, 0);

        for(int i = 0; i< arrowNo; i++)
        {
            arrows[i] = gameObject.transform.GetChild(0).GetChild(i).gameObject;
            arrows[i].GetComponent<OscillatingMovement>().amplitude = baseVector[i] * amplitude;
        }
    }

    void Update()
    {
        if (targetUnit != null && targetUnit.Alive) {
            transform.position = targetUnit.gameObject.transform.position;
            gameObject.makeVisible();
        }
        else
        {
            gameObject.makeInvisible();
        }
    }

    public void SetTargetUnit(Unit unit)
    {
        if (unit == targetUnit) return;

        targetUnit = unit;

        if (targetUnit != null)
        {
            SetSize(unit.Size + minSize);
        }
    }

    public void SetSize(float size)
    {
        for(int i = 0; i< arrowNo; i++)
        {
            arrows[i].GetComponent<OscillatingMovement>().begin = baseVector[i] * size;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldUI : MonoBehaviour {

    Unit unit;
    GameObject movementIndicator;
    
    void Start () {
        unit = GetComponent<Unit>();

        if (movementIndicator == null)
        {
            movementIndicator = Instantiate(Resources.Load("UI/World/MovementIndicator/MovementIndicator"), gameObject.transform) as GameObject;
        }
    }

    void Update()
    {
        updateMovementIndicator();
    }

    public void updateMovementIndicator()
    {
        MovingState movingState = unit.MovingState;

        if (movingState.IsActive)
        {
            movementIndicator.SetActive(true);
            Vector3 pos = movingState.MoveDest;
            pos.y = 0.01f;
            movementIndicator.transform.position = pos;
        }
        else
        {
            movementIndicator.SetActive(false);
        }

    }
}

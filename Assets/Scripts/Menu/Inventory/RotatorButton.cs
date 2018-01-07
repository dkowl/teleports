using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatorButton : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 90;
    [SerializeField] private Direction direction;
    [SerializeField] protected Transform rotationTarget;
    private RepeatButton button;

    private void Awake()
    {
        button = GetComponent<RepeatButton>();
        Debug.Assert(button != null);
    }

    private void Update()
    {
        if (button.IsHeld)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        if (rotationTarget != null)
        {
            rotationTarget.Rotate(RotationDirection(direction) * Time.deltaTime * 360);
        }
    }

    private Vector3 RotationDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                return new Vector3(0, 1, 0);
            case Direction.Left:
                return new Vector3(0, -1, 0);
            default:
                return Vector3.zero;
        }
    }

    public enum Direction
    {
        Left,
        Right
    }
}

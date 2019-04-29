using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public enum Axis
    {
        None = 0,
        Up,
        Down,
        Back,
        Front,
        Left,
        Right,
    }

    public Axis RotationAxis = Axis.None;
    public float AngleSpeed = 1;

    private void FixedUpdate()
    {
        switch(RotationAxis)
        {
            case Axis.Up:
                transform.Rotate(Vector3.up, AngleSpeed);
                break;
            case Axis.Down:
                transform.Rotate(Vector3.down, AngleSpeed);
                break;
            case Axis.Back:
                transform.Rotate(Vector3.back, AngleSpeed);
                break;
            case Axis.Front:
                transform.Rotate(Vector3.forward, AngleSpeed);
                break;
            case Axis.Left:
                transform.Rotate(Vector3.left, AngleSpeed);
                break;
            case Axis.Right:
                transform.Rotate(Vector3.right, AngleSpeed);
                break;
            default:
                break;
        }
    }
}

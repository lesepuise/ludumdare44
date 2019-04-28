using System;
using CleverCode;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _pole;
    [SerializeField] private Transform _pivot;

    [Range(0f, 1f)]
    [SerializeField] private float _distanceLerp;

    private float _currentDistance;
    private float distance
    {
        get { return _currentDistance; }
        set
        {
            _pivot.transform.localPosition = Vector3.back * value;
            _currentDistance = value;
        }
    }

    [NonSerialized]
    public float targetDistance;

    public void Update()
    {
        CheckMaxAngle();

        UpdateDistance();
    }

    private void CheckMaxAngle()
    {
        //TO BE DONE
        //raycast to make sure the camera can go low enough without going through the ground
        //find a valid angle
    }

    private void UpdateDistance()
    {
        float currentDist = _currentDistance;
        float targetDist = targetDistance;

        float lerp = 1 - Mathf.Pow(1 - _distanceLerp, Time.deltaTime * 10);

        distance = Mathf.Lerp(currentDist, targetDist, lerp);
    }

    public Vector3 GetForward()
    {
        return _pivot.forward;
    }

    public Vector3 GetRight()
    {
        return _pivot.right;
    }
}
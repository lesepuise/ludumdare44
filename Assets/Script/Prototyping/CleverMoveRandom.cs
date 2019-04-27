using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;

public class CleverMoveRandom : MonoBehaviour
{
    private Vector3 _basePosition;

    private Vector3 _offset;
    private Vector3 _direction;

    private Vector3 _offsetTarget;

    public float speed = 1;
    public float maxOffset = 10;

    public float directionMoverTime = 1f;
    
    public Vector3 axeMultiplier = Vector3.one;

    public bool randomizeOnStart;

    private Coroutine _directionMover = null;
    private float _limiter = 1f;

    private float _minMovementSqr;

    public void SetLimiter(float t)
    {
        _limiter = Mathf.Clamp01(t);
    }

    private void Awake()
    {
        float moveRatio = maxOffset / speed;
        float minMovement = directionMoverTime / moveRatio;
        minMovement *= 1.33f;

        minMovement = Mathf.Min(0.75f, minMovement);
        _minMovementSqr = minMovement * minMovement;

        _basePosition = transform.localPosition;

        RandomizeStart();
    }

    private void Update()
    {
        if (_directionMover == null)
        {
            _directionMover = StartCoroutine(MoveDirectionCo());
        }

        UpdateOffset();

        transform.localPosition = _basePosition + GetFinalOffset();
    }

    private void OnDisable()
    {
        _directionMover = null;
    }

    private void UpdateOffset()
    {
        Vector3 oldOffset = _offset;
        _offset += _direction * speed * Time.deltaTime;

        if (_offset.sqrMagnitude > maxOffset * maxOffset)
        {
            _offset = oldOffset;
        }
    }

    private IEnumerator MoveDirectionCo()
    {
        _offsetTarget = GenerateNewDirectionTarget();

        float t = 0;

        float step = 1 / directionMoverTime;

        Vector3 start = _direction;
        Vector3 targetDirection = _offsetTarget - _offset;

        while (t < 1)
        {
            yield return new WaitForFixedUpdate();

            t += Time.fixedDeltaTime * step;

            _direction = Vector3.Lerp(start, targetDirection, CurveType.SmoothLinear.Get(t));
        }

        _directionMover = null;
    }

    private Vector3 GenerateNewDirectionTarget()
    {
        Vector3 target;

        while (true)
        {
            target = Random.insideUnitSphere;

            if ((target - _offsetTarget).sqrMagnitude > _minMovementSqr)
            {
                break;
            }
        }

        return target;
    }

    private void RandomizeStart()
    {
        if (!randomizeOnStart)
        {
            return;
        }

        _offset = Random.insideUnitSphere * maxOffset;
    }

    private Vector3 GetFinalOffset()
    {
        return Vector3.Scale(_offset, axeMultiplier) * _limiter;
    }
}
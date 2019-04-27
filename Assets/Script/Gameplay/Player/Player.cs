using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Transform _ball;

    [Header("Camera")] [SerializeField] private Camera _camera;
    [SerializeField] private Transform _cameraPole;

    [SerializeField] private float _cameraBaseDist;
    [SerializeField] private float _cameraDistPerSize;
    [SerializeField] private float _cameraDistPerSpeed;

    private float Strength => PlayerData.Instance.GetStrength();
    private float StartSize => PlayerData.Instance.GetSize();
    private float JumpStrength => 5f;

    private float _currentSize;

    public float CurrentSize
    {
        get { return _currentSize; }
        set
        {
            _currentSize = value;
            SetScale(_currentSize);
        }
    }

    public float CurrentSpeed
    {
        get { return _rigidBody.velocity.magnitude; }
    }

    private void Start()
    {
        PlayerData.Instance.RecalculateAll();

        InitBall();
    }

    private void InitBall()
    {
        CurrentSize = StartSize;

        transform.position += Vector3.up * StartSize / 2f;
    }

    private void Update()
    {
        UpdateCamera();

        UpdateControls();
    }

    #region Controls

    private void UpdateControls()
    {
        UpdateJump();
        UpdateMovement();
    }

    private bool CanJump()
    {
        return IsGrounded();
    }

    private void UpdateJump()
    {
        if (!CanJump())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidBody.AddForce(Vector3.up * GetJumpStrength(), ForceMode.VelocityChange);
        }
    }

    private float GetJumpStrength()
    {
        return 10f;
    }

    private void UpdateMovement()
    {
        Vector3 movement = Vector3.zero;

        if (UpKey) movement += GetForward();
        if (LeftKey) movement += -GetRight();
        if (DownKey) movement += -GetForward();
        if (RightKey) movement += GetRight();

        _rigidBody.AddForce(movement * Strength, ForceMode.Acceleration);
    }

    #region Keys

    private bool UpKey => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    private bool LeftKey => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    private bool DownKey => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    private bool RightKey => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

    #endregion

    private bool IsGrounded()
    {
        int layerMask = 1 << Layer.Scenery;
        layerMask += 1 << Layer.Obstacle;

        float minDist = CurrentSize * 0.05f;
        Vector3 targetPos = transform.position + Vector3.down * minDist;

        if (Physics.CheckSphere(targetPos, CurrentSize, layerMask, QueryTriggerInteraction.Collide))
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Camera

    private Vector3 GetForward()
    {
        return _cameraPole.forward;
    }

    private Vector3 GetRight()
    {
        return _cameraPole.right;
    }

    private void UpdateCamera()
    {
        _camera.transform.localPosition = Vector3.back * GetTargetDistance();
    }

    private float GetTargetDistance()
    {
        float dist = _cameraBaseDist;

        dist += CurrentSize * _cameraDistPerSize;
        dist += CurrentSpeed * _cameraDistPerSpeed;

        return dist;
    }

    #endregion

    #region Size

    private void SetScale(float newScale)
    {
        _ball.localScale = Vector3.one * newScale;
    }

    #endregion
}
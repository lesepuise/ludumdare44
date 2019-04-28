using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private Transform _ball;
    [SerializeField] private Transform _spikes;

    [SerializeField] private PlayerCamera _camera;

    [SerializeField] private float _cameraBaseDist;
    [SerializeField] private float _cameraDistPerSize;
    [SerializeField] private float _cameraDistPerSpeed;
    [SerializeField] private float _cameraDistMinSpeed = 2;

    [SerializeField] private float _cameraDistDebug = 20;

    private bool Spiky => _spikes.gameObject.activeSelf;

    private float MaxSpeed => PlayerData.Instance.GetMaxSpeed();
    private float Strength => PlayerData.Instance.GetStrength() * (Spiky ? 2f : 1f);
    private float StartSize => PlayerData.Instance.GetSize();
    private float JumpStrength => 10f;

    private float _currentSize;

    public float GetCurrentSize()
    {
        return _currentSize;
    }

    public void SetCurrentSize(float value)
    {
        _currentSize = value;
        SetScale(_currentSize);
    }

    public float GetCurrentHeight()
    {
        return LevelManager.Instance.GetHeight(transform);
    }

    private void OnEnable()
    {
        PlayerData.Instance.RegisterPlayer(this);
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            PlayerData.Instance.UnregisterPlayer();
        }
    }

    public float GetCurrentSpeed()
    {
        return _rigidBody.velocity.magnitude;
    }

    private void Start()
    {
        PlayerData.Instance.RecalculateAll();

        InitBall();

        _spikes.gameObject.SetActive(false);
    }

    private void InitBall()
    {
        SetCurrentSize(StartSize);

        transform.position += Vector3.up * StartSize / 2f;
    }

    private void Update()
    {
        UpdateCamera();
        UpdateControls();
        UpdateCheats();

        UpdateVelocity();
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
        return JumpStrength;
    }

    private void UpdateMovement()
    {
        if (!IsGrounded())
        {
            return;
        }

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

        float minDist = GetCurrentSize() * 0.05f;
        if (Spiky)
        {
            minDist += GetCurrentSize() * 0.05f;
        }

        Vector3 targetPos = transform.position + Vector3.down * minDist;

        if (Physics.CheckSphere(targetPos, GetCurrentSize(), layerMask, QueryTriggerInteraction.Collide))
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Velocity

    private void UpdateVelocity()
    {
        Vector3 vel = _rigidBody.velocity;
        Vector3 velHorizontal = vel;
        velHorizontal.y = 0f;

        Vector3 velVertical = vel - velHorizontal;

        float speed = velHorizontal.magnitude;

        if (speed >= MaxSpeed)
        {
            _rigidBody.velocity = velHorizontal / speed * MaxSpeed + velVertical;
        }
    }

    #endregion

    #region Camera

    private Vector3 GetForward()
    {
        return _camera.GetForward();
    }

    private Vector3 GetRight()
    {
        return _camera.GetRight();
    }

    private void UpdateCamera()
    {
        _camera.targetDistance = GetTargetDistance();
    }

    private float GetTargetDistance()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            return _cameraDistDebug;
        }

        float dist = _cameraBaseDist;

        dist += GetCurrentSize() * _cameraDistPerSize;
        dist += Mathf.Max(_cameraDistMinSpeed, GetCurrentSpeed() * _cameraDistPerSpeed);
        
        return dist;
    }

    #endregion

    #region Size

    private void SetScale(float newScale)
    {
        _ball.localScale = Vector3.one * newScale;
    }

    #endregion

    #region Cheats

    private void UpdateCheats()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _spikes.gameObject.SetActive(!Spiky);
        }
    }

    #endregion

    #region Size

    public float GetCurrentWeight()
    {
        return 1f;
    }

    #endregion
}
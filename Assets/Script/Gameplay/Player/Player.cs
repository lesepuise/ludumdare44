using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float Strength => PlayerData.Instance.GetStrength();
    private float StartSize => PlayerData.Instance.GetSize();

    private float WeightRatio => PlayerData.Instance.GetWeightRatio();
    private float JumpStrength => 10f;

    private float _currentSize;
    private bool _paused;

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
        UpdateCheats();
    }

    private void FixedUpdate()
    {
        UpdateWeight();

        UpdatePhysicState();

        UpdateControls();
        UpdateVelocity();

        UpdateLastMovements();
        UpdateLife();
    }

    public void Pause()
    {
        _paused = true;
    }

    #region Physical State

    private float _verticalVel;

    public bool IsGrounded { get; private set; }

    private void UpdateGrounded()
    {
        bool wasGrounded = IsGrounded;
        float lastVerticalVel = _verticalVel;

        IsGrounded = _collisionCount > 0 && _lastContactNormal.y > 0;
        _verticalVel = _rigidBody.velocity.y;

        if (!wasGrounded && IsGrounded)
        {
            Debug.Log("Hit the ground! -- Last Vel : " + lastVerticalVel + " New Vel : " + _verticalVel);
        }
    }

    private void UpdatePhysicState()
    {
        return;

        bool wasGrounded = IsGrounded;

        IsGrounded = CheckIfGrounded();
        _verticalVel = _rigidBody.velocity.y;
    }

    #endregion

    #region Controls

    private bool CanControl()
    {
        return !_paused;
    }

    private void UpdateControls()
    {
        if (!CanControl())
        {
            return;
        }

        UpdateJump();
        UpdateMovement();
    }

    private bool CanJump()
    {
        return IsGrounded && !Spiky;
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

    private float GetStrengthFactor()
    {
        float factor = 1f;

        if (Spiky) factor *= 2f;
        if (!IsGrounded) factor *= 0.5f;

        return factor;
    }

    private void UpdateMovement()
    {
        float strenghtFactor = GetStrengthFactor();

        Vector3 movement = Vector3.zero;

        if (UpKey) movement += GetForward();
        if (LeftKey) movement += -GetRight();
        if (DownKey) movement += -GetForward();
        if (RightKey) movement += GetRight();

        _rigidBody.AddForce(movement * Strength * strenghtFactor, ForceMode.Force);
    }

    #region Keys

    private bool UpKey => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    private bool LeftKey => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    private bool DownKey => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    private bool RightKey => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

    #endregion

    private bool CheckIfGrounded()
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

    #region Weight

    public float GetCurrentWeight()
    {
        return GetCurrentSize() * WeightRatio;
    }

    public float UpdateWeight()
    {
        return _rigidBody.mass = GetCurrentWeight();
    }

    #endregion

    #region Life

    //To be, or not to be

    private void UpdateLife()
    {
        if (IsGrounded)
        {

        }
    }

    private void LoseLife(float lifeToLose)
    {

    }

    private void LoseLifePercent(float percentToLose)
    {
        
    }

    #endregion

    #region Last Movements

    private List<TimedMovement> _lastMovements = new List<TimedMovement>();
    private bool _startedMoving = false;

    private void UpdateLastMovements()
    {
        while (_lastMovements.Sum(movement => movement.time) > 2f)
        {
            _lastMovements.RemoveAt(0);
        }

        _lastMovements.Add(new TimedMovement(Time.deltaTime, _rigidBody.velocity.magnitude));

        if (!_startedMoving && _lastMovements.LastElement().movement > 0.05f)
        {
            _startedMoving = true;
            _lastMovements.Clear();
        }
    }

    public bool LastMovementsValid()
    {
        return _startedMoving && _lastMovements.Sum(movement => movement.time) > 1f;
    }

    public float GetLastSecondMovement()
    {
        return _lastMovements.Sum(movement => movement.movement);
    }

    private struct TimedMovement
    {
        public float time;
        public float movement;

        public TimedMovement(float time, float movement)
        {
            this.time = time;
            this.movement = movement;
        }
    }

    #endregion

    #region Collision Management

    private int _collisionCount;
    private Vector3 _lastContactNormal;

    private void CalculateCollisionNormal(Collision col)
    {
        Vector3 normals = Vector3.zero;

        for (int i = 0; i < col.contactCount; i++)
        {
            normals += col.GetContact(i).normal;
        }

        _lastContactNormal = normals / col.contactCount;
    }

    private bool IsCollisionValid(Collision col)
    {
        bool layer = col.gameObject.layer == Layer.Scenery || col.gameObject.layer == Layer.Obstacle;

        return layer;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (IsCollisionValid(col))
        {
            CalculateCollisionNormal(col);
            _collisionCount++;
        }

        UpdateGrounded();
    }

    private void OnCollisionStay(Collision col)
    {
        if (IsCollisionValid(col))
        {
            CalculateCollisionNormal(col);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (IsCollisionValid(col))
        {
            _collisionCount--;
        }

        UpdateGrounded();
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
}
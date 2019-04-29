using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CleverCode;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [Header("Parts")] [SerializeField] private Transform _ball;
    [SerializeField] private Transform _spikes;
    [FormerlySerializedAs("_rigidBody")] public Rigidbody RigidBody;

    [Header("Camera")] [SerializeField] private PlayerCamera _camera;
    [SerializeField] private AudioSource _landSound;

    [SerializeField] private AudioSource _hitSound;

    [SerializeField] private AudioSource _jumpSound;

    [SerializeField] private float _cameraBaseDist;
    [SerializeField] private float _cameraDistPerSize;
    [SerializeField] private float _cameraDistPerSpeed;
    [SerializeField] private float _cameraDistMinSpeed = 2;

    [SerializeField] private float _cameraDistDebug = 20;

    private bool Spiky => _spikes.gameObject.activeSelf;

    private float MaxSpeed => PlayerData.Instance.GetMaxSpeed(GetCurrentSize());
    private float Strength => PlayerData.Instance.GetStrength();
    private float StartSize => PlayerData.Instance.GetStartSize();
    private float LifeLossFactor => PlayerData.Instance.GetLifeLossFactor();
    private float LifeGainFactor => PlayerData.Instance.GetLifeGainFactor();
    private float JumpCost => PlayerData.Instance.GetJumpCost();

    private float WeightRatio => PlayerData.Instance.GetWeightRatio();
    private float JumpStrength => PlayerData.Instance.GetJumpStrength();

    private float _life;

    private float _currentSize;
    private bool _paused;

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
        return RigidBody.velocity.magnitude;
    }

    public float GetHorizontalSpeed()
    {
        Vector3 horizontalVel = RigidBody.velocity;
        horizontalVel.y = 0f;

        return horizontalVel.magnitude;
    }

    public float GetForwardSpeed()
    {
        Vector3 horizontalVel = RigidBody.velocity;
        horizontalVel.y = 0f;

        float forwardFactor = Mathf.Max(0, Vector3.Dot(horizontalVel.normalized, GetForward()));

        return horizontalVel.magnitude * forwardFactor;
    }

    private void Start()
    {
        PlayerData.Instance.RecalculateAll();

        InitBall();

        _spikes.gameObject.SetActive(false);
    }

    private void InitBall()
    {
        SetLifeFromSize(StartSize);

        IsGainingSnow = false;
    }

    private void Update()
    {
        UpdateJump(); //cannot be in fixed update, need to check input GetKeyDown, only valid in main loop

        UpdateCamera();
        UpdateCheats();
    }

    private void FixedUpdate()
    {
        UpdateWeight();

        UpdatePhysicState();

        UpdateMovement();
        UpdateVelocity();

        UpdateLastMovements();
        UpdateLifeOnMoving();
    }

    public void InitCamera()
    {
        _camera.Init();
    }

    public void Pause()
    {
        _paused = true;
    }

    public void Unpause()
    {
        _paused = false;
    }

    public void PausePhysic()
    {
        RigidBody.isKinematic = true;
    }

    public void UnpausePhysic()
    {
        RigidBody.isKinematic = false;
    }

    #region Physical State

    private float _verticalVel;

    public bool IsGrounded { get; private set; }
    public bool IsGainingSnow { get; set; }
    public bool OnIce { get; set; }

    private void UpdateGrounded()
    {
        bool wasGrounded = IsGrounded;
        float lastVerticalVel = _verticalVel;

        IsGrounded = _collisionCount > 0 && _lastContactNormal.y > 0;
        _verticalVel = RigidBody.velocity.y;

        if (!wasGrounded && IsGrounded)
        {
            OnHitTheGround(_verticalVel - lastVerticalVel);
        }
    }

    private void OnHitTheGround(float impactStrength)
    {
        if (impactStrength > 9f) //big impact
        {
            //TODO : Lose life, big time
            _landSound.Play();
        }
        else if (impactStrength > 4f) //medium Impact
        {
            //TODO : Lose life, just a bit
            _landSound.Play();
        }
        else if (impactStrength > 1f) //Small Impact
        {
            //TODO : Lose life? nah, you're fine
        }
    }

    private void UpdatePhysicState()
    {
        _verticalVel = RigidBody.velocity.y;
    }

    #endregion

    #region Controls

    private bool CanControl()
    {
        return !_paused;
    }

    private bool CanJump()
    {
        return IsGrounded && !Spiky;
    }

    private void UpdateJump()
    {
        if (!CanControl() || !CanJump())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void Jump()
    {
        _jumpSound.Play();

        LoseLifePercent(JumpCost);

        RigidBody.AddForce(_lastContactNormal * GetJumpStrength(), ForceMode.VelocityChange);
    }

    private float GetJumpStrength()
    {
        return JumpStrength;
    }

    private float GetStrengthFactor()
    {
        if (OnIce) return 0f;
        float factor = 1f;

        if (Spiky) factor *= 2f;
        if (!IsGrounded) factor *= 0.5f;

        return factor;
    }

    private void UpdateMovement()
    {
        if (!CanControl())
        {
            return;
        }

        float strenghtFactor = GetStrengthFactor();

        Vector3 movement = Vector3.zero;

        if (LeftKey) movement += -GetRight();
        if (RightKey) movement += GetRight();

        if (UpKey)
        {
            float ratio = Mathf.Max(0, 1 - GetForwardSpeed() / MaxSpeed);

            movement += GetForward() * ratio;
        }

        if (DownKey)
        {
            float ratio = CurveType.FakeSqrt.Get(GetForwardSpeed() / MaxSpeed);

            movement += -GetForward() * ratio;
        }

        RigidBody.AddForce(movement * Strength * strenghtFactor * GetCurrentSize(), ForceMode.Force);

        //Set the right tacks to play depending of the speed
        MusicManager.Instance.setPlayerSpeed(GetHorizontalSpeed());
    }

    #region Keys

    private bool UpKey => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
    private bool LeftKey => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
    private bool DownKey => Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
    private bool RightKey => Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

    #endregion

    #endregion

    #region Velocity

    private void UpdateVelocity()
    {
        if (_currentSize < 0.025f) //fallback
        {
            RigidBody.velocity = Vector3.zero;
        }

        Vector3 vel = RigidBody.velocity;
        Vector3 velHorizontal = vel;
        velHorizontal.y = 0f;

        Vector3 velVertical = vel - velHorizontal;

        float speed = velHorizontal.magnitude;

        if (speed >= MaxSpeed)
        {
            RigidBody.velocity = velHorizontal / speed * MaxSpeed + velVertical;
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

    public void RecalculateSize()
    {
        SetCurrentSize(PlayerData.LifeToSize(_life));
    }

    public float GetCurrentSize()
    {
        return _currentSize;
    }

    public void SetCurrentSize(float value)
    {
        _currentSize = value;
        SetScale(_currentSize);

        UpdateWeight();
    }

    private void SetScale(float newScale)
    {
        _ball.localScale = Vector3.one * newScale;
    }

    #endregion

    #region Weight

    public float GetCurrentWeight()
    {
        return GetLife() * WeightRatio / PlayerData.LifeFactor;
    }

    public float UpdateWeight()
    {
        return RigidBody.mass = GetCurrentWeight();
    }

    #endregion

    #region Life

    //To be, or not to be

    private void SetLifeFromSize(float startSize)
    {
        SetLife(PlayerData.SizeToLife(startSize));
    }

    private void UpdateLifeOnMoving()
    {
        if (!LevelManager.Instance.GameInProgress) return;
        if (!IsGrounded) return;
        if (Spiky) return;

        float lifeToLoose = 0;
        if (IsGainingSnow)
        {
            lifeToLoose = RigidBody.velocity.magnitude * GetCurrentSize() * LifeGainFactor;
        }
        else
        {
            lifeToLoose = RigidBody.velocity.magnitude * GetCurrentSize() * LifeLossFactor;
        }

        LoseLife(lifeToLoose);
    }

    private void LoseLife(float lifeToLose)
    {
        SetLife(_life - lifeToLose);
    }

    private void LoseLifePercent(float percentToLose)
    {
        SetLife(_life - _life * percentToLose);
    }

    private void OnLifeLoss(float lostLife)
    {
    }

    private void SetLife(float newLife)
    {
        newLife = Mathf.Max(0, newLife);

        float lastLife = _life;
        _life = newLife;

        if (newLife < lastLife)
        {
            OnLifeLoss(lastLife - newLife);
        }

        RecalculateSize();
    }

    public float GetLife()
    {
        return _life;
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

        _lastMovements.Add(new TimedMovement(Time.deltaTime, RigidBody.velocity.magnitude));

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

    private void OnRemoveSpikes()
    {
        _collisionCount = 0;
    }

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
            _collisionCount = Mathf.Max(0, _collisionCount);
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

            if (!Spiky)
            {
                OnRemoveSpikes();
            }
        }
    }

    #endregion
}
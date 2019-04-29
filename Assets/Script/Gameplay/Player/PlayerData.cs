using CleverCode;
using System;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public const float LifeFactor = 1000f;
    public const float SizeLifeFactor = 5f;

    [NonSerialized] public Player CurrentPlayer;

    public GameObject snowPatchPrefab;

    [SerializeField] private float _baseStrength = 1f;
    [SerializeField] private float _baseJumpStrength = 10f;

    [SerializeField] private float _baseSize = 0.2f;
    [SerializeField] private float _baseMaxSpeed = 10f;

    [SerializeField] private float _baseLifeLossFactor = 1f;
    [SerializeField] private float _baseLifeGainFactor = -2f;
    [SerializeField] private float _baseJumpCost = 0.5f;
    [SerializeField] private float _hitCost = 0.20f;

    public void RegisterPlayer(Player player)
    {
        CurrentPlayer = player;
    }

    public void UnregisterPlayer()
    {
        CurrentPlayer = null;
    }

    protected override void Init()
    {
        RecalculateAll();
    }

    public void RecalculateAll()
    {
        CalculateStrength();
        CalculateSize();
        CalculateMaxSpeed();

        CalculateWeightRatio();
        CalculateLifeCosts();
        CalculateJumpStrength();
    }

    #region Strength

    private float _calculatedStrength;

    public float GetStrength()
    {
        return _calculatedStrength;
    }

    private void CalculateStrength()
    {
        _calculatedStrength = _baseStrength;

        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectStrength(ref _calculatedStrength));
    }

    #endregion

    #region Weight Ratio

    private float _calculatedWeightRatio;

    public float GetWeightRatio()
    {
        return _calculatedWeightRatio;
    }

    private void CalculateWeightRatio()
    {
        _calculatedWeightRatio = 1f;

        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectWeightRatio(ref _calculatedWeightRatio));
    }

    #endregion

    #region Max Speed

    private float _calculatedMaxSpeed;

    public float GetMaxSpeed(float currentSize)
    {
        float ratio = Mathf.Sqrt(currentSize);

        return _calculatedMaxSpeed * ratio;
    }

    private void CalculateMaxSpeed()
    {
        _calculatedMaxSpeed = _baseMaxSpeed;

        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectMaxSpeed(ref _calculatedMaxSpeed));
    }

    #endregion

    #region Jump Strength

    private float _calculatedJumpStrength;

    public float GetJumpStrength()
    {
        return _calculatedJumpStrength;
    }

    private void CalculateJumpStrength()
    {
        _calculatedJumpStrength = _baseJumpStrength;

        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectJumpStrength(ref _calculatedJumpStrength));
    }

    #endregion

    #region Starting Size

    private float _calculatedSize;

    public float GetStartSize()
    {
        return _calculatedSize;
    }

    private void CalculateSize()
    {
        _calculatedSize = _baseSize;

        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectStartSize(ref _calculatedSize));
    }

    #endregion

    #region Life

    private float _lifeLossFactor;
    private float _lifeGainFactor;
    private float _jumpCost;

    public float GetLifeLossFactor()
    {
        return _lifeLossFactor;
    }

    public float GetLifeGainFactor()
    {
        return _lifeGainFactor;
    }

    public float GetJumpCost()
    {
        return _jumpCost;
    }

    public float GetHitCost()
    {
        return _hitCost;
    }

    private void CalculateLifeCosts()
    {
        _lifeLossFactor = _baseLifeLossFactor;
        _lifeGainFactor = _baseLifeGainFactor;
        _jumpCost = _baseJumpCost;

        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectLifeLossFactor( ref _lifeLossFactor));
        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectLifeLossFactor( ref _lifeGainFactor));
        PowerupManager.Instance.PassivePowers.GetPassivePowers().ForEach(power => power.AffectJumpCost(ref _jumpCost));
    }

    #endregion

    #region Helper Function

    public static float SizeToLife(float size)
    {
        size *= SizeLifeFactor;
        float life = size * size;
        return life * LifeFactor;
    }

    public static float LifeToSize(float life)
    {
        life /= LifeFactor;
        life = Mathf.Sqrt(life);
        life /= SizeLifeFactor;

        return life;
    }

    #endregion
}
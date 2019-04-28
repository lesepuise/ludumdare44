using System;
using CleverCode;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    private const float LifeFactor = 5000f;

    [NonSerialized] public Player CurrentPlayer;

    [SerializeField] private float _baseStrength = 1f;
    [SerializeField] private float _baseJumpStrength = 10f;

    [SerializeField] private float _baseSize = 0.2f;
    [SerializeField] private float _baseMaxSpeed = 10f;

    [SerializeField] private float _baseLifeLossFactor = 1f;
    [SerializeField] private float _baseLifeGainFactor = -2f;
    [SerializeField] private float _baseJumpCost = 0.5f;

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

    public float GetSize()
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

    private const float SphereRatio = Mathf.PI * 1.3333f;

    public static float SizeToLife(float size)
    {
        float life = size * size * size * SphereRatio;

        return life * LifeFactor;
    }

    public static float LifeToSize(float life)
    {
        life /= LifeFactor;
        life /= SphereRatio;

        return Mathf.Pow(life, 0.33333f);
    }

    #endregion
}
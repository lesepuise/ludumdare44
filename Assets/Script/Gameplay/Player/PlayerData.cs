using CleverCode;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public Player CurrentPlayer;

    [SerializeField] private float _baseStrength = 1f;
    [SerializeField] private float _baseSize = 0.2f;
    [SerializeField] private float _baseMaxSpeed = 10f;

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
    }

    #endregion

    #region Max Speed

    private float _calculatedMaxSpeed;

    public float GetMaxSpeed()
    {
        return _calculatedMaxSpeed;
    }

    private void CalculateMaxSpeed()
    {
        _calculatedMaxSpeed = _baseMaxSpeed;
    }

    #endregion

    #region Jump Strength

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
    }

    #endregion
}
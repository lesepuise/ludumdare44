using CleverCode;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    [SerializeField] private float _baseStrength = 1f;
    [SerializeField] private float _baseSize = 0.2f;

    protected override void Init()
    {
        RecalculateAll();
    }

    public void RecalculateAll()
    {
        CalculateStrength();
        CalculateSize();
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

    #region Jump Strenght


    
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
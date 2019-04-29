using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string LevelName;

    public int LevelVariant;
    public int Cost;
    public int Reward;
    public float RewardFactor;
    public int TotalHeight;
}
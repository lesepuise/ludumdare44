using CleverCode;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int chosenVariant = 0;

    public Player playerPrefab;

    public float maxMovementForDeath = 0.4f;

    public int chosenLauncherIndex = 1;

    public int metaSnow;
}
using CleverCode;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static void StartGameCorrectly()
    {
        SceneManager.LoadScene(0);
    }
}
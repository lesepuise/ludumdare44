using CleverCode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    private const string LEVEL_01_NAME = "Level_BasicMountain";   
    private const string LEVEL_02_NAME = "Level_Volcano";

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            GameStarter.StartGameCorrectly();
            return;
        }
        MusicManager.Instance.SetScene(1);
    }

    public void Button_Start()
    {
        SceneManager.LoadScene(LEVEL_01_NAME);
    }

    public void Button_Quit()
    {
        SceneManager.LoadScene(1);
    }
}
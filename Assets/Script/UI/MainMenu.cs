using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance == null)
        {
            GameStarter.StartGameCorrectly();
            return;
        }
        MusicManager.Instance.SetScene(0);
    }

    public void Button_Start()
    {
        SceneManager.LoadScene(2);
    }

    public void Button_Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
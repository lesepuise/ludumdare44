﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.Instance == null)
        {
            GameManager.StartGameCorrectly();
        }
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
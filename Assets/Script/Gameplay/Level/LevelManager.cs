using System;
using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private StartingPoint _startingPoint;
    [SerializeField] private Transform _targetPoint;

    private bool _gameInProgress => !won && !lost;

    [NonSerialized]
    public bool won;

    [NonSerialized]
    public bool lost;

    protected override void Init()
    {
        if (GameManager.Instance == null)
        {
            GameStarter.StartGameCorrectly();
            return;
        }
        MusicManager.Instance.SetScene(2);
        SpawnPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitLevel();
        }

        GameLoop();
    }

    private void GameLoop()
    {
        if (!_gameInProgress)
        {
            return;
        }

        Player player = PlayerData.Instance.CurrentPlayer;

        if (player == null)
        {
            return;
        }

        if (player.GetCurrentHeight() <= 0.5f)
        {
            StartVictorySequence();
            return;
        }

        if (player.GetCurrentSize() <= 0.05f)
        {
            StartDefeatSequence();
            return;
        }

        if (player.LastMovementsValid() && player.GetLastSecondMovement() < GameManager.Instance.maxMovementForDeath)
        {
            StartDefeatSequence();
        }
    }

    private void StartVictorySequence()
    {
        won = true;

        PlayerHud.Instance.StartVictorySequence();

        PlayerData.Instance.CurrentPlayer.Pause();
    }

    private void StartDefeatSequence()
    {
        lost = true;

        PlayerHud.Instance.StartDefeatSequence();

        PlayerData.Instance.CurrentPlayer.Pause();
    }

    private void SpawnPlayer()
    {
        _startingPoint.SpawnPlayer();
    }

    #region Height Management

    public float GetHeight(Transform target)
    {
        return target.transform.position.y - _targetPoint.position.y;
    }

    public float GetHeight(Vector3 target)
    {
        return target.y - _targetPoint.position.y;
    }

    public float GetHeight(float y)
    {
        return y - _targetPoint.position.y;
    }

    #endregion

    public int CalculatePoints()
    {
        return 999;
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(2);
    }
}
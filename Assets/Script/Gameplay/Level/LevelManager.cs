using System;
using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelManager : Singleton<LevelManager>
{
    public LevelData levelData;

    [FormerlySerializedAs("_startingPoint")] [SerializeField] private StartingPlatform _startingPlatform;
    [SerializeField] private Transform _targetPoint;

    public bool GameInProgress => !won && !lost;

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
        if (!GameInProgress)
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
        _startingPlatform.SpawnPlayer();
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
        return (int)(PlayerData.Instance.CurrentPlayer.GetLife() * levelData.RewardFactor + levelData.Reward);
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(2);
    }
}
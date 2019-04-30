using System;
using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelManager : Singleton<LevelManager>
{
    public LevelData levelDataVar_01;
    public LevelData levelDataVar_02;
    public LevelData levelDataVar_03;

    [FormerlySerializedAs("_startingPoint")]
    [SerializeField] private StartingPlatform _startingPlatform;
    public Transform TargetPoint { get {
        switch (GameManager.Instance.chosenVariant)
        {
                default: return _targetPoint1;
                case 1: return _targetPoint2;
                case 2: return _targetPoint3;
        }
    } }
    [SerializeField] private Transform _targetPoint1;
    [SerializeField] private Transform _targetPoint2;
    [SerializeField] private Transform _targetPoint3;

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
        MusicManager.Instance.Pause();
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
        return target.transform.position.y - TargetPoint.position.y;
    }

    public float GetHeight(Vector3 target)
    {
        return target.y - TargetPoint.position.y;
    }

    public float GetHeight(float y)
    {
        return y - TargetPoint.position.y;
    }

    #endregion

    public int CalculatePoints()
    {
        LevelData levelData = GetVariant();

        return (int)(PlayerData.Instance.CurrentPlayer.GetLife() * levelData.RewardFactor + levelData.Reward);
    }

    public LevelData GetVariant()
    {
        switch (GameManager.Instance.chosenVariant)
        {
            case 0: return levelDataVar_01;
            case 1: return levelDataVar_02;
            case 2: return levelDataVar_03;
            default: return levelDataVar_01;
        }
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(2);
    }
}
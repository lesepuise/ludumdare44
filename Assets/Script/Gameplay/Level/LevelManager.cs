using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private StartingPoint _startingPoint;
    [SerializeField] private Transform _targetPoint;

    protected override void Init()
    {
        if (GameManager.Instance == null)
        {
            GameStarter.StartGameCorrectly();
            return;
        }

        SpawnPlayer();
    }

    private void Update()
    {
        GameLoop();
    }

    private void GameLoop()
    {
        
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
}
﻿using System;
using System.Collections;
using System.Collections.Generic;
using CleverCode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : Singleton<PlayerHud>
{
    [SerializeField] private GameObject _stats;
    [SerializeField] private StatBlock _statBlockTemplate;

    [Header("Level Completion")]
    [SerializeField] private GameObject _endingScreen;
    [SerializeField] private GameObject _endingWon;
    [SerializeField] private GameObject _endingLost;
    [SerializeField] private Text _points;

    private void Start()
    {
        _statBlockTemplate.gameObject.SetActive(false);
        _endingScreen.gameObject.SetActive(false);

        InitStat();
    }

    private void InitStat()
    {
        Player player = PlayerData.Instance.CurrentPlayer;

        SpawnStat("Speed", player.GetCurrentSpeed);
        SpawnStat("Height", player.GetCurrentHeight);

        SpawnStat("Size", player.GetCurrentSize);
        SpawnStat("Weight", player.GetCurrentWeight);
    }

    private void SpawnStat(string statName, Func<float> StatGetter)
    {
        StatBlock newStat = Instantiate(_statBlockTemplate, _statBlockTemplate.transform.parent);
        newStat.Init(statName, StatGetter);
        newStat.gameObject.SetActive(true);
    }

    private void Update()
    {
        _stats.gameObject.SetActive(PlayerData.Instance.CurrentPlayer);
    }

    public void StartVictorySequence()
    {
        HideStats();
        ShowEndScreen(true);
    }

    public void StartDefeatSequence()
    {
        HideStats();
        ShowEndScreen(false);
    }

    private void HideStats()
    {
        _stats.SetActive(false);
    }

    private void ShowEndScreen(bool won)
    {
        _endingScreen.SetActive(true);

        _endingWon.SetActive(won);
        _endingLost.SetActive(!won);

        StartCoroutine(ShowPoints(LevelManager.Instance.CalculatePoints()));
    }

    private IEnumerator ShowPoints(int points)
    {
        float shownPoints = 0;

        float startTime = Time.time;

        while (shownPoints < points)
        {
            yield return null;

            float currentTime = Time.time - startTime;
            currentTime /= 5f;
            currentTime *= currentTime;

            shownPoints += Mathf.Max(1f, currentTime);
            shownPoints = Math.Min(shownPoints, points);

            SetPoints(shownPoints);
        }
    }

    private void SetPoints(float points)
    {
        _points.text = points.ToString("F0").PadLeft(6, '0');
    }

    public void Button_CompleteLevel()
    {
        LevelManager.Instance.QuitLevel();
    }
}
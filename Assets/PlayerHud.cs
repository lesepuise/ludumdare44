using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private GameObject _stats;

    [SerializeField] private StatBlock _statBlockTemplate;

    private void Start()
    {
        _statBlockTemplate.gameObject.SetActive(false);

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
}

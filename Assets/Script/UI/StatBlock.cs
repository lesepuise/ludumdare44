using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBlock : MonoBehaviour
{
    public Text label;
    public Text value;

    public Func<float> getter;

    public void Init(string statName, Func<float> getter)
    {
        label.text = statName;

        this.getter = getter;
    }

    public void Update()
    {
        if (PlayerData.Instance.CurrentPlayer)
        {
            value.text = getter().ToString("F1");
        }
    }
}
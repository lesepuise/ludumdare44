using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBlock : MonoBehaviour
{
    public Text label;
    public Text value;

    public Func<float> getter;
    public Func<string> stringGetter;

    public void Init(string statName, Func<float> getter)
    {
        label.text = statName;

        this.getter = getter;
    }

    public void Init(string statName, Func<string> getter)
    {
        label.text = statName;

        stringGetter = getter;
    }

    public void Update()
    {
        if (PlayerData.Instance.CurrentPlayer)
        {
            if (getter != null)
            {
                value.text = getter().ToString("F1");
            }

            if (stringGetter != null)
            {
                value.text = stringGetter();
            }
        }
    }
}
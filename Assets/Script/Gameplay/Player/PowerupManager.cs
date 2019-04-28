using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CleverCode;

public class PowerupManager : Singleton<PowerupManager>
{
    public PassivePowerList PassivePowers;
    public ActivePowerList ActivePowers;
}

[Serializable]
public abstract class PowerList
{
    public abstract List<Power> GetPowers();
}

[Serializable]
public class Power
{
    public string powerName = "Unnamed Power";

    public int cost = 100;
    public bool purchased = false;
}
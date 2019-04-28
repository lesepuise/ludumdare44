using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActivePowerList : PowerList
{
    public IceBomb IceBomb;

    public override List<Power> GetPowers()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public abstract class ActivePower : Power
{
    public KeyCode actionKey;

    public int maxUse = 100000;

    [NonSerialized]
    public int useLeft;

    public void Init()
    {
        useLeft = maxUse;
    }

    public void Update()
    {
        if (!purchased || useLeft == 0) return;

        if (Input.GetKeyDown(actionKey))
        {
            Activate();
        }
    }

    protected abstract void Activate();
}

[Serializable]
public class IceBomb : ActivePower
{
    protected override void Activate()
    {
        throw new NotImplementedException();
    }
}
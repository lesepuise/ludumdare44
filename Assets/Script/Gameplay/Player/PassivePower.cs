using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassivePowerList : PowerList
{
    public PassiveStrength PassiveStrength;
    public PassiveStrength PassiveStrength2;
    public PassiveStrength PassiveStrength3;

    public PassiveJumpStrength PassiveJumpStrength;
    public PassiveJumpStrength PassiveJumpStrength2;
    public PassiveJumpStrength PassiveJumpStrength3;

    public PassiveStartSize PassiveStartSize;
    public PassiveStartSize PassiveStartSize2;
    public PassiveStartSize PassiveStartSize3;

    public PassiveMaxSpeed PassiveMaxSpeed;
    public PassiveMaxSpeed PassiveMaxSpeed2;
    public PassiveMaxSpeed PassiveMaxSpeed3;

    public override List<Power> GetPowers()
    {
        return GetPassivePowers().ConvertAll(power => power as Power);
    }

    public List<PassivePower> GetPassivePowers()
    {
        return new List<PassivePower>
        {
            PassiveStrength,
            PassiveStrength2,
            PassiveStrength3,
            PassiveJumpStrength,
            PassiveJumpStrength2,
            PassiveJumpStrength3,
            PassiveStartSize,
            PassiveStartSize2,
            PassiveStartSize3,
            PassiveMaxSpeed,
            PassiveMaxSpeed2,
            PassiveMaxSpeed3,
        };
    }
}

[Serializable]
public class PassivePower : Power
{
    public virtual void AffectStrength(ref float value)
    {
    }

    public virtual void AffectJumpStrength(ref float value)
    {
    }

    public virtual void AffectStartSize(ref float value)
    {
    }

    public virtual void AffectMaxSpeed(ref float value)
    {
    }

    public virtual void AffectLifeLossFactor(ref float value)
    {
    }

    public virtual void AffectJumpCost(ref float value)
    {
    }

    public virtual void AffectWeightRatio(ref float value)
    {
    }

    public virtual void Affect(ref float value)
    {
    }
}

[Serializable]
public class PassiveStrength : PassivePower
{
    public float Factor = 1.5f;

    public override void AffectStrength(ref float value)
    {
        if (!purchased) return;
        value *= Factor;
    }
}

[Serializable]
public class PassiveJumpStrength : PassivePower
{
    public float Factor = 1.5f;

    public override void AffectJumpStrength(ref float value)
    {
        if (!purchased) return;
        value *= Factor;
    }
}

[Serializable]
public class PassiveStartSize : PassivePower
{
    public float Factor = 1.5f;

    public override void AffectStartSize(ref float value)
    {
        if (!purchased) return;
        value *= Factor;
    }
}

[Serializable]
public class PassiveMaxSpeed : PassivePower
{
    public float Factor = 1.5f;

    public override void AffectMaxSpeed(ref float value)
    {
        if (!purchased) return;
        value *= Factor;
    }
}

[Serializable]
public class Passive : PassivePower
{
}
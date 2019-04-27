using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCode
{
    public enum Axe
    {
        X,
        Y,
        Z
    }

    public enum CurveType
    {
        Sqrt = 0,
        FakeSqrt = 1,
        Linear = 2,
        Squared = 3,
        Cubed = 4,
        Parabola = 5,
        SmoothLinear = 6,
        SmoothEnd = 7,

        SquaredEaseOut = 8,
        CubedEaseOut = 9,
    }

    public enum TimeType
    {
        Normal,
        Fixed,
        Unscaled,
    }
}
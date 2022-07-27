using System;

namespace Graphal.RubiksCube.Core
{
    [Flags]
    public enum CubeDimension
    {
        None = 0,
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        Top = 16,
        Bottom = 32,
        MiddleSouthNorth = 64,
        MiddleWestEast = 128,
        MiddleTopBottom = 256,
    }
}
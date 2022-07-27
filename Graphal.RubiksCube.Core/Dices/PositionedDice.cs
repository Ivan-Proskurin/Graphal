using System;
using System.Collections.Generic;

using Graphal.RubiksCube.Core.Extensions;

namespace Graphal.RubiksCube.Core.Dices
{
    public class PositionedDice
    {
        public CubeDimension Position { get; set; }

        public RubiksDice Dice { get; set; }
        
        private readonly Dictionary<CubeDimension, CubeDimension> _northDirectionMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.East | CubeDimension.Top] = CubeDimension.East | CubeDimension.Bottom,
            [CubeDimension.East | CubeDimension.MiddleTopBottom] = CubeDimension.Bottom | CubeDimension.MiddleWestEast,
            [CubeDimension.Bottom | CubeDimension.East] = CubeDimension.Bottom | CubeDimension.West,
            [CubeDimension.Bottom | CubeDimension.MiddleWestEast] = CubeDimension.West | CubeDimension.MiddleTopBottom,
            [CubeDimension.Bottom | CubeDimension.West] = CubeDimension.Top | CubeDimension.West,
            [CubeDimension.West | CubeDimension.MiddleTopBottom] = CubeDimension.Top | CubeDimension.MiddleWestEast,
            [CubeDimension.Top | CubeDimension.West] = CubeDimension.Top | CubeDimension.East,
            [CubeDimension.Top | CubeDimension.MiddleWestEast] = CubeDimension.East | CubeDimension.MiddleTopBottom,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _topDirectionMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.East | CubeDimension.South] = CubeDimension.East | CubeDimension.North,
            [CubeDimension.East | CubeDimension.MiddleSouthNorth] = CubeDimension.North | CubeDimension.MiddleWestEast,
            [CubeDimension.East | CubeDimension.North] = CubeDimension.West | CubeDimension.North,
            [CubeDimension.North | CubeDimension.MiddleWestEast] = CubeDimension.West | CubeDimension.MiddleSouthNorth,
            [CubeDimension.North | CubeDimension.West] = CubeDimension.South | CubeDimension.West,
            [CubeDimension.West | CubeDimension.MiddleSouthNorth] = CubeDimension.South | CubeDimension.MiddleWestEast,
            [CubeDimension.West | CubeDimension.South] = CubeDimension.East | CubeDimension.South,
            [CubeDimension.South | CubeDimension.MiddleWestEast] = CubeDimension.East | CubeDimension.MiddleSouthNorth,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _westDirectionMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.South | CubeDimension.Bottom] = CubeDimension.South | CubeDimension.Top,
            [CubeDimension.South | CubeDimension.MiddleTopBottom] = CubeDimension.Top | CubeDimension.MiddleSouthNorth,
            [CubeDimension.South | CubeDimension.Top] = CubeDimension.Top | CubeDimension.North,
            [CubeDimension.Top | CubeDimension.MiddleSouthNorth] = CubeDimension.North | CubeDimension.MiddleTopBottom,
            [CubeDimension.Top | CubeDimension.North] = CubeDimension.Bottom | CubeDimension.North,
            [CubeDimension.North | CubeDimension.MiddleTopBottom] = CubeDimension.Bottom | CubeDimension.MiddleSouthNorth,
            [CubeDimension.Bottom | CubeDimension.North] = CubeDimension.South | CubeDimension.Bottom,
            [CubeDimension.Bottom | CubeDimension.MiddleSouthNorth] = CubeDimension.South | CubeDimension.MiddleTopBottom,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _westEastDirectoryMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.South | CubeDimension.Bottom] = CubeDimension.South | CubeDimension.Top,
            [CubeDimension.South | CubeDimension.MiddleTopBottom] = CubeDimension.Top | CubeDimension.MiddleSouthNorth,
            [CubeDimension.South | CubeDimension.Top] = CubeDimension.Top | CubeDimension.North,
            [CubeDimension.Top | CubeDimension.MiddleSouthNorth] = CubeDimension.North | CubeDimension.MiddleTopBottom,
            [CubeDimension.Top | CubeDimension.North] = CubeDimension.Bottom | CubeDimension.North,
            [CubeDimension.North | CubeDimension.MiddleTopBottom] = CubeDimension.Bottom | CubeDimension.MiddleSouthNorth,
            [CubeDimension.Bottom | CubeDimension.North] = CubeDimension.South | CubeDimension.Bottom,
            [CubeDimension.Bottom | CubeDimension.MiddleSouthNorth] = CubeDimension.South | CubeDimension.MiddleTopBottom,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _eastDirectoryMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.North | CubeDimension.Bottom] = CubeDimension.North | CubeDimension.Top,
            [CubeDimension.North | CubeDimension.MiddleTopBottom] = CubeDimension.Top | CubeDimension.MiddleSouthNorth,
            [CubeDimension.Top | CubeDimension.North] = CubeDimension.South | CubeDimension.Top,
            [CubeDimension.Top | CubeDimension.MiddleSouthNorth] = CubeDimension.South | CubeDimension.MiddleTopBottom,
            [CubeDimension.South | CubeDimension.Top] = CubeDimension.Bottom | CubeDimension.South,
            [CubeDimension.South | CubeDimension.MiddleTopBottom] = CubeDimension.Bottom | CubeDimension.MiddleSouthNorth,
            [CubeDimension.Bottom | CubeDimension.South] = CubeDimension.North | CubeDimension.Bottom,
            [CubeDimension.Bottom | CubeDimension.MiddleSouthNorth] = CubeDimension.North | CubeDimension.MiddleTopBottom,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _bottomDirectoryMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.East | CubeDimension.North] = CubeDimension.East | CubeDimension.South,
            [CubeDimension.East | CubeDimension.MiddleSouthNorth] = CubeDimension.South | CubeDimension.MiddleWestEast,
            [CubeDimension.East | CubeDimension.South] = CubeDimension.West | CubeDimension.South,
            [CubeDimension.South | CubeDimension.MiddleWestEast] = CubeDimension.West | CubeDimension.MiddleSouthNorth,
            [CubeDimension.West | CubeDimension.South] = CubeDimension.North | CubeDimension.West,
            [CubeDimension.West | CubeDimension.MiddleSouthNorth] = CubeDimension.North | CubeDimension.MiddleWestEast,
            [CubeDimension.North | CubeDimension.West] = CubeDimension.East | CubeDimension.North,
            [CubeDimension.North | CubeDimension.MiddleWestEast] = CubeDimension.East | CubeDimension.MiddleSouthNorth,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _southNorthDirectoryMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.East | CubeDimension.Bottom] = CubeDimension.East | CubeDimension.Top,
            [CubeDimension.East | CubeDimension.MiddleTopBottom] = CubeDimension.Top | CubeDimension.MiddleWestEast,
            [CubeDimension.East | CubeDimension.Top] = CubeDimension.West | CubeDimension.Top,
            [CubeDimension.Top | CubeDimension.MiddleWestEast] = CubeDimension.West | CubeDimension.MiddleTopBottom,
            [CubeDimension.West | CubeDimension.Top] = CubeDimension.Bottom | CubeDimension.West,
            [CubeDimension.West | CubeDimension.MiddleTopBottom] = CubeDimension.Bottom | CubeDimension.MiddleWestEast,
            [CubeDimension.Bottom | CubeDimension.West] = CubeDimension.East | CubeDimension.Bottom,
            [CubeDimension.Bottom | CubeDimension.MiddleWestEast] = CubeDimension.East | CubeDimension.MiddleTopBottom,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _southDirectionMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.East | CubeDimension.Bottom] = CubeDimension.East | CubeDimension.Top,
            [CubeDimension.East | CubeDimension.MiddleTopBottom] = CubeDimension.Top | CubeDimension.MiddleWestEast,
            [CubeDimension.East | CubeDimension.Top] = CubeDimension.West | CubeDimension.Top,
            [CubeDimension.Top | CubeDimension.MiddleWestEast] = CubeDimension.West | CubeDimension.MiddleTopBottom,
            [CubeDimension.West | CubeDimension.Top] = CubeDimension.Bottom | CubeDimension.West,
            [CubeDimension.West | CubeDimension.MiddleTopBottom] = CubeDimension.Bottom | CubeDimension.MiddleWestEast,
            [CubeDimension.Bottom | CubeDimension.West] = CubeDimension.East | CubeDimension.Bottom,
            [CubeDimension.Bottom | CubeDimension.MiddleWestEast] = CubeDimension.East | CubeDimension.MiddleTopBottom,
        };
        
        private readonly Dictionary<CubeDimension, CubeDimension> _topBottomDirectionMatrix = new Dictionary<CubeDimension, CubeDimension>
        {
            [CubeDimension.East | CubeDimension.South] = CubeDimension.East | CubeDimension.North,
            [CubeDimension.East | CubeDimension.MiddleSouthNorth] = CubeDimension.North | CubeDimension.MiddleWestEast,
            [CubeDimension.East | CubeDimension.North] = CubeDimension.North | CubeDimension.West,
            [CubeDimension.North | CubeDimension.MiddleWestEast] = CubeDimension.West | CubeDimension.MiddleSouthNorth,
            [CubeDimension.West | CubeDimension.North] = CubeDimension.South | CubeDimension.West,
            [CubeDimension.West | CubeDimension.MiddleSouthNorth] = CubeDimension.South | CubeDimension.MiddleWestEast,
            [CubeDimension.South | CubeDimension.West] = CubeDimension.East | CubeDimension.South,
            [CubeDimension.South | CubeDimension.MiddleWestEast] = CubeDimension.East | CubeDimension.MiddleSouthNorth,
        };

        private static Dictionary<CubeDimension, CubeDimension> GetReverseDirectionMatrix(
            Dictionary<CubeDimension, CubeDimension> matrix)
        {
            var reverseDirectionMatrix = new Dictionary<CubeDimension, CubeDimension>();
            foreach (var dimension in matrix)
            {
                reverseDirectionMatrix[dimension.Value] = dimension.Key;
            }

            return reverseDirectionMatrix;
        }

        public void ShiftDicePosition(CubeDimension direction, bool reverse)
        {
            if (!Position.Contains(direction))
            {
                throw new ArgumentException("Can not move dice is this direction");
            }

            var directionsMatrixes = new Dictionary<CubeDimension, Dictionary<CubeDimension, CubeDimension>>
            {
                [CubeDimension.North] = _northDirectionMatrix,
                [CubeDimension.Top] = _topDirectionMatrix,
                [CubeDimension.West] = _westDirectionMatrix,
                [CubeDimension.MiddleWestEast] = _westEastDirectoryMatrix,
                [CubeDimension.East] = _eastDirectoryMatrix,
                [CubeDimension.Bottom] = GetReverseDirectionMatrix(_bottomDirectoryMatrix),
                [CubeDimension.MiddleSouthNorth] = _southNorthDirectoryMatrix,
                [CubeDimension.South] = _southDirectionMatrix,
                [CubeDimension.MiddleTopBottom] = _topBottomDirectionMatrix,
            };

            var position = Position.Subtract(direction);
            var directionMatrix = directionsMatrixes[direction]; 
            directionMatrix = reverse ? GetReverseDirectionMatrix(directionMatrix) : directionMatrix;
            if (directionMatrix.TryGetValue(position, out var newPosition))
            {
                Position = newPosition | direction;
            }
        }
    }
}
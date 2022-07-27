using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;
using Graphal.RubiksCube.Core.Dices;
using Graphal.RubiksCube.Core.Extensions;

namespace Graphal.RubiksCube.Core
{
    public class RubikCube : Object3D
    {
        // RubiksCube Colors
        public static readonly Color BackgroundColor = Color.Chocolate;
        public static readonly Color WestColor = Color.Maroon;
        public static readonly Color EastColor = Color.Gold;
        public static readonly Color NorthColor = Color.Lime;
        public static readonly Color SouthColor = Color.Aqua;
        public static readonly Color TopColor = Color.Beige;
        public static readonly Color BottomColor = Color.Indigo;
        
        private const int DiceSize = 100;
        private const int DiceGap = 5;
        private const int DiceOffset = DiceSize + DiceGap;
        
        private Vector3D _position;
        private readonly List<PositionedDice> _dices = new List<PositionedDice>();
        private readonly Dictionary<CubeDimension, PlaneRotationInfo> _rotateVectors = new Dictionary<CubeDimension, PlaneRotationInfo>();

        public RubikCube(Vector3D position)
        {
            _position = position;
            InitDices();
        }

        private void InitDices()
        {
            var orthogonalDimensions1 = new[] { CubeDimension.South, CubeDimension.MiddleSouthNorth, CubeDimension.North };
            var orthogonalDimensions2 = new[] { CubeDimension.West, CubeDimension.MiddleWestEast, CubeDimension.East };
            var orthogonalDimensions3 = new[] { CubeDimension.Bottom, CubeDimension.MiddleTopBottom, CubeDimension.Top };

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        var dimension1 = orthogonalDimensions1[i];
                        var dimension2 = orthogonalDimensions2[j];
                        var dimension3 = orthogonalDimensions3[k];
                        
                        var dimensionSet = dimension1 | dimension2 | dimension3;

                        var x = dimensionSet.Contains(CubeDimension.West) ? -DiceOffset :
                            dimensionSet.Contains(CubeDimension.East) ? DiceOffset : 0;
                        var y = dimensionSet.Contains(CubeDimension.Top) ? -DiceOffset :
                            dimensionSet.Contains(CubeDimension.Bottom) ? DiceOffset : 0;
                        var z = dimensionSet.Contains(CubeDimension.North) ? DiceOffset :
                            dimensionSet.Contains(CubeDimension.South) ? -DiceOffset : 0;
                        
                        var positionedDice = new PositionedDice
                        {
                            Position = dimensionSet,
                            Dice = new RubiksDice(DiceSize, _position.Add(new Vector3D(x, y, z)),
                                new[] {dimension1, dimension2, dimension3}),
                        };
                        _dices.Add(positionedDice);
                    }
                }
            }

            InitRotateVectors();
        }

        private void InitRotateVectors()
        {
            _rotateVectors[CubeDimension.Bottom] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, DiceOffset, 0)).ToVector3DR(),
                Vector = new Vector3DR(0, 1, 0),
            };
            _rotateVectors[CubeDimension.East] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(DiceOffset, 0, 0)).ToVector3DR(),
                Vector = new Vector3DR(1, 0, 0),
            };
            _rotateVectors[CubeDimension.North] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, 0, DiceOffset)).ToVector3DR(),
                Vector = new Vector3DR(0, 0, 1),
            };
            _rotateVectors[CubeDimension.South] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, 0, -DiceOffset)).ToVector3DR(),
                Vector = new Vector3DR(0, 0, -1),
            };
            _rotateVectors[CubeDimension.Top] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, -DiceOffset, 0)).ToVector3DR(),
                Vector = new Vector3DR(0, -1, 0),
            };
            _rotateVectors[CubeDimension.West] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(-DiceOffset, 0, 0)).ToVector3DR(),
                Vector = new Vector3DR(-1, 0, 0),
            };
            _rotateVectors[CubeDimension.MiddleSouthNorth] = new PlaneRotationInfo
            {
                Position = _position.ToVector3DR(),
                Vector = new Vector3DR(0, 0, -1),
            };
            _rotateVectors[CubeDimension.MiddleTopBottom] = new PlaneRotationInfo
            {
                Position = _position.ToVector3DR(),
                Vector = new Vector3DR(0, -1, 0),
            };
            _rotateVectors[CubeDimension.MiddleWestEast] = new PlaneRotationInfo
            {
                Position = _position.ToVector3DR(),
                Vector = new Vector3DR(-1, 0, 0),
            };
        }

        private void RotateDimension(CubeDimension dimension, bool reverse)
        {
            var rotationDices = _dices.Where(x => x.Position.IsInDimension(dimension)).ToArray();
            if (rotationDices.Length > 9)
            {
                throw new ArgumentException(dimension.ToString());
            }
            rotationDices.ForEach(x => x.ShiftDicePosition(dimension, reverse));
            
            // Start rotation animation
            var rotationInfo = _rotateVectors[dimension];
            var triangles = rotationDices.SelectMany(x => x.Dice.GetTriangles());
            foreach (var triangle in triangles)
            {
                triangle.RotateAroundVector(rotationInfo.Position, rotationInfo.Vector, reverse ? -Math.PI / 2 : Math.PI / 2);
            }
            // End rotation animation
        }

        public override IEnumerable<Triangle3D> GetTriangles()
        {
            return _dices.SelectMany(x => x.Dice.GetTriangles());
        }

        public override void RotateCubeDimension(bool reverse)
        {
            RotateDimension(CubeDimension.MiddleTopBottom, reverse);
        }

        public override void RotateCubeDimension(int cubeDimension, bool reverse)
        {
            RotateDimension((CubeDimension)cubeDimension, reverse);
        }

        public override void MoveCloserByGrade(double grade)
        {
            var direction = _position.Multiply(-grade);
            _position = _position.Add(direction);
            ApplyToRotationVectors(x =>
            {
                var d = x.Position.Multiply(-grade);
                x.Position = x.Position.Add(d);
            });
        }

        public override void MoveFurtherByGrade(double grade)
        {
            var direction = _position.Multiply(grade);
            _position = _position.Add(direction);
            ApplyToRotationVectors(x =>
            {
                var d = x.Position.Multiply(grade);
                x.Position = x.Position.Add(d);
            });
        }

        private void ApplyToRotationVectors(Action<PlaneRotationInfo> action)
        {
            foreach (var vector in _rotateVectors.Values)
            {
                action(vector);
            }
        }

        public override void StartRotation()
        {
            ApplyToRotationVectors(x => x.StartRotation());
        }

        public override void Rotate(double radiansXZ, double radiansYZ)
        {
            ApplyToRotationVectors(x => x.Rotate(_position.ToVector3DR(), radiansXZ, radiansYZ));
        }

        public override void StopRotation()
        {
            ApplyToRotationVectors(x => x.StopRotation());
        }
    }
}
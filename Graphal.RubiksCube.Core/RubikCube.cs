using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Graphal.Engine.ThreeD.Colorimetry;
using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;
using Graphal.RubiksCube.Core.Dices;

namespace Graphal.RubiksCube.Core
{
    public class RubikCube
    {
        private const int DiceSize = 100;
        private const int DiceGap = 5;
        private const int DiceOffset = DiceSize + DiceGap;
        public static readonly Color BackgroundColor = Color.Chocolate;
        
        private Vector3D _position;
        private readonly RubiksDice[,,] _dices = new RubiksDice[3, 3, 3];

        private readonly Dictionary<CubePlane, CubePlaneDescriptor> _dicesByPlanes =
            new Dictionary<CubePlane, CubePlaneDescriptor>();
        private readonly Dictionary<CubePlane, PlaneRotationInfo> _rotateVectors = new Dictionary<CubePlane, PlaneRotationInfo>();

        public RubikCube(Vector3D position)
        {
            _position = position;
            InitDices();
        }

        private void InitDices()
        {
            // i - плоскость в глубину
            // j - плоскость снизу вверх
            // k - плоскость слева направо

            // _dices[1, 1, 1] = new RubiksDice(DiceSize, _position);

            _dices[1, 2, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, -DiceOffset, 0)));
            _dices[1, 2, 1].AddFacet(FacetOrientation.Top, Color.Beige);
            
            _dices[1, 1, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, 0, 0)));
            _dices[1, 1, 2].AddFacet(FacetOrientation.East, Color.Gold);
            
            _dices[0, 1, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, 0, -DiceOffset)));
            _dices[0, 1, 1].AddFacet(FacetOrientation.South, Color.Aqua);
            
            _dices[1, 0, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, DiceOffset, 0)));
            _dices[1, 0, 1].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            
            _dices[1, 1, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, 0, 0)));
            _dices[1, 1, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            
            _dices[2, 1, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, 0, DiceOffset)));
            _dices[2, 1, 1].AddFacet(FacetOrientation.North, Color.Lime);
            
            _dices[0, 1, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, 0, -DiceOffset)));
            _dices[0, 1, 2].AddFacet(FacetOrientation.South, Color.Aqua);
            _dices[0, 1, 2].AddFacet(FacetOrientation.East, Color.Gold);
            
            _dices[0, 1, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, 0, -DiceOffset)));
            _dices[0, 1, 0].AddFacet(FacetOrientation.South, Color.Aqua);
            _dices[0, 1, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            
            _dices[0, 0, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, DiceOffset, -DiceOffset)));
            _dices[0, 0, 0].AddFacet(FacetOrientation.South, Color.Aqua);
            _dices[0, 0, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            _dices[0, 0, 0].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            
            _dices[0, 0, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, DiceOffset, -DiceOffset)));
            _dices[0, 0, 1].AddFacet(FacetOrientation.South, Color.Aqua);
            _dices[0, 0, 1].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            
            _dices[0, 0, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, DiceOffset, -DiceOffset)));
            _dices[0, 0, 2].AddFacet(FacetOrientation.South, Color.Aqua);
            _dices[0, 0, 2].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            _dices[0, 0, 2].AddFacet(FacetOrientation.East, Color.Gold);
            
            _dices[2, 1, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, 0, DiceOffset)));
            _dices[2, 1, 2].AddFacet(FacetOrientation.East, Color.Gold);
            _dices[2, 1, 2].AddFacet(FacetOrientation.North, Color.Lime);
            
            _dices[2, 1, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, 0, DiceOffset)));
            _dices[2, 1, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            _dices[2, 1, 0].AddFacet(FacetOrientation.North, Color.Lime);
            
            _dices[1, 0, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, DiceOffset, 0)));
            _dices[1, 0, 2].AddFacet(FacetOrientation.East, Color.Gold);
            _dices[1, 0, 2].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            
            _dices[2, 0, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, DiceOffset, DiceOffset)));
            _dices[2, 0, 2].AddFacet(FacetOrientation.East, Color.Gold);
            _dices[2, 0, 2].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            _dices[2, 0, 2].AddFacet(FacetOrientation.North, Color.Lime);
            
            _dices[2, 0, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, DiceOffset, DiceOffset)));
            _dices[2, 0, 1].AddFacet(FacetOrientation.North, Color.Lime);
            _dices[2, 0, 1].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            
            _dices[2, 0, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, DiceOffset, DiceOffset)));
            _dices[2, 0, 0].AddFacet(FacetOrientation.North, Color.Lime);
            _dices[2, 0, 0].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            _dices[2, 0, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            
            _dices[1, 0, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, DiceOffset, 0)));
            _dices[1, 0, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            _dices[1, 0, 0].AddFacet(FacetOrientation.Bottom, Color.Indigo);
            
            _dices[0, 2, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, -DiceOffset, -DiceOffset)));
            _dices[0, 2, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            _dices[0, 2, 0].AddFacet(FacetOrientation.Top, Color.Beige);
            _dices[0, 2, 0].AddFacet(FacetOrientation.South, Color.Aqua);
            
            _dices[0, 2, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, -DiceOffset, -DiceOffset)));
            _dices[0, 2, 1].AddFacet(FacetOrientation.Top, Color.Beige);
            _dices[0, 2, 1].AddFacet(FacetOrientation.South, Color.Aqua);
            
            _dices[0, 2, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, -DiceOffset, -DiceOffset)));
            _dices[0, 2, 2].AddFacet(FacetOrientation.East, Color.Gold);
            _dices[0, 2, 2].AddFacet(FacetOrientation.Top, Color.Beige);
            _dices[0, 2, 2].AddFacet(FacetOrientation.South, Color.Aqua);
            
            _dices[1, 2, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, -DiceOffset, 0)));
            _dices[1, 2, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            _dices[1, 2, 0].AddFacet(FacetOrientation.Top, Color.Beige);
            
            _dices[1, 2, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, -DiceOffset, 0)));
            _dices[1, 2, 2].AddFacet(FacetOrientation.East, Color.Gold);
            _dices[1, 2, 2].AddFacet(FacetOrientation.Top, Color.Beige);

            _dices[2, 2, 0] = new RubiksDice(DiceSize, _position.Add(new Vector3D(-DiceOffset, -DiceOffset, DiceOffset)));
            _dices[2, 2, 0].AddFacet(FacetOrientation.West, Color.Maroon);
            _dices[2, 2, 0].AddFacet(FacetOrientation.North, Color.Lime);
            _dices[2, 2, 0].AddFacet(FacetOrientation.Top, Color.Beige);
            
            _dices[2, 2, 1] = new RubiksDice(DiceSize, _position.Add(new Vector3D(0, -DiceOffset, DiceOffset)));
            _dices[2, 2, 1].AddFacet(FacetOrientation.North, Color.Lime);
            _dices[2, 2, 1].AddFacet(FacetOrientation.Top, Color.Beige);
            
            _dices[2, 2, 2] = new RubiksDice(DiceSize, _position.Add(new Vector3D(DiceOffset, -DiceOffset, DiceOffset)));
            _dices[2, 2, 2].AddFacet(FacetOrientation.North, Color.Lime);
            _dices[2, 2, 2].AddFacet(FacetOrientation.Top, Color.Beige);
            _dices[2, 2, 2].AddFacet(FacetOrientation.East, Color.Gold);

            
            // i - плоскость в глубину
            // j - плоскость снизу вверх
            // k - плоскость слева направо
            
            InitRotateVectors();
            InitDicesByPlanes();
        }

        private void InitRotateVectors()
        {
            _rotateVectors[CubePlane.Bottom] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, DiceOffset, 0)).ToVector3DR(),
                Vector = _position.Add(new Vector3D(0, DiceOffset, 0)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.East] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(DiceOffset, 0, 0)).ToVector3DR(),
                Vector = _position.Add(new Vector3D(DiceOffset, 0, 0)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.North] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, 0, DiceOffset)).ToVector3DR(),
                Vector = _position.Add(new Vector3D(0, 0, DiceOffset)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.South] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, 0, -DiceOffset)).ToVector3DR(),
                Vector = _position.Add(new Vector3D(0, 0, -DiceOffset)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.Top] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(0, -DiceOffset, 0)).ToVector3DR(),
                Vector = _position.Add(new Vector3D(0, -DiceOffset, 0)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.West] = new PlaneRotationInfo
            {
                Position = _position.Add(new Vector3D(-DiceOffset, 0, 0)).ToVector3DR(),
                Vector = _position.Add(new Vector3D(-DiceOffset, 0, 0)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.MiddleSouthNorth] = new PlaneRotationInfo
            {
                Position = _position.ToVector3DR(),
                Vector = _position.Add(new Vector3D(0, 0, -DiceOffset)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.MiddleTopBottom] = new PlaneRotationInfo
            {
                Position = _position.ToVector3DR(),
                Vector = _position.Add(new Vector3D(0, -DiceOffset, 0)).ToVector3DR(),
            };
            _rotateVectors[CubePlane.MiddleWestEast] = new PlaneRotationInfo
            {
                Position = _position.ToVector3DR(),
                Vector = _position.Add(new Vector3D(-DiceOffset, 0, 0)).ToVector3DR(),
            };
        }

        private void InitDicesByPlanes()
        {
            UpdateSouthDices();
            UpdateNorthDices();
            UpdateBottomDices();
        }

        private void UpdatePlaneDices(CubePlane plane, Func<List<RubiksDice>> getDices)
        {
            _dicesByPlanes[plane] = new CubePlaneDescriptor
            {
                Plane = plane,
                RotationInfo = _rotateVectors[plane],
                Dices = getDices().ToList(),
            };
        }

        private void UpdateSouthDices()
        {
            // i - плоскость в глубину
            // j - плоскость снизу вверх
            // k - плоскость слева направо
            UpdatePlaneDices(CubePlane.South, () =>
            {
                var dices = new List<RubiksDice>();
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        dices.Add(_dices[0, j, k]); 
                    }
                }

                return dices;
            });
        }

        private void UpdateNorthDices()
        {
            // i - плоскость в глубину
            // j - плоскость снизу вверх
            // k - плоскость слева направо
            UpdatePlaneDices(CubePlane.North, () =>
            {
                var dices = new List<RubiksDice>();
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        dices.Add(_dices[0, j, k]);
                    }
                }

                return dices;
            });
        }

        private void UpdateBottomDices()
        {
            UpdatePlaneDices(CubePlane.Bottom, () =>
            {
                var dices = new List<RubiksDice>();
                for (var i = 0; i < 3; i++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        dices.Add(_dices[i, 0, k]);
                    }
                }

                return dices;
            });
        }

        public IEnumerable<Triangle3D> GetTriangles()
        {
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        var dice = _dices[i, j, k];
                        if (dice == null) continue;
                        using (var enumerator = dice.GetTriangles().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                yield return enumerator.Current;
                            }
                        }
                    }
                }
            }
        }
    }
}
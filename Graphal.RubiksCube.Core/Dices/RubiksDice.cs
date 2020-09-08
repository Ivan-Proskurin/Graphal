using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;

namespace Graphal.RubiksCube.Core.Dices
{
    public class RubiksDice
    {
        private readonly int _size;
        private readonly Vector3D _position;
        private readonly Dictionary<FacetOrientation, CubeColorFacet> _facets = new Dictionary<FacetOrientation, CubeColorFacet>();

        public RubiksDice(int size, Vector3D position)
        {
            _size = size;
            _position = position;
            InitDefaultFacets();
        }

        public void AddFacet(FacetOrientation orientation, Color color)
        {
            _facets[orientation] = CreateFacet(color, orientation, GetFacetInvertion(orientation));
        }

        public IEnumerable<Triangle3D> GetTriangles()
        {
            return _facets.Values.SelectMany(x => x);
        }

        private void InitDefaultFacets()
        {
            foreach (var orientationObj in Enum.GetValues(typeof(FacetOrientation)))
            {
                var orientation = (FacetOrientation) orientationObj;
                _facets[orientation] = CreateFacet(null, orientation, GetFacetInvertion(orientation));
            }
        }

        private CubeColorFacet CreateFacet(Color? color, FacetOrientation orientation, bool invertion)
        {
            return new CubeColorFacet(_size, GetFacetPosition(orientation), GetFacetDimensions(orientation), color, invertion);
        }

        private Vector3D GetFacetPosition(FacetOrientation orientation)
        {
            switch (orientation)
            {
                case FacetOrientation.West:
                    return new Vector3D(_position.X - _size / 2, _position.Y, _position.Z);
                case FacetOrientation.East:
                    return new Vector3D(_position.X + _size / 2, _position.Y, _position.Z);
                case FacetOrientation.South:
                    return new Vector3D(_position.X, _position.Y, _position.Z - _size / 2);
                case FacetOrientation.North:
                    return new Vector3D(_position.X, _position.Y, _position.Z + _size / 2);
                case FacetOrientation.Top:
                    return new Vector3D(_position.X, _position.Y - _size / 2, _position.Z);
                case FacetOrientation.Bottom:
                    return new Vector3D(_position.X, _position.Y + _size / 2, _position.Z);
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }

        private static FacetDimensions GetFacetDimensions(FacetOrientation orientation)
        {
            switch (orientation)
            {
                case FacetOrientation.West:
                case FacetOrientation.East:
                    return FacetDimensions.YZ; 
                case FacetOrientation.North:
                case FacetOrientation.South:
                    return FacetDimensions.XY;
                case FacetOrientation.Top:
                case FacetOrientation.Bottom:
                    return FacetDimensions.XZ;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }

        private static bool GetFacetInvertion(FacetOrientation orientation)
        {
            switch (orientation)
            {
                case FacetOrientation.West:
                    return true;
                case FacetOrientation.East:
                    return false;
                case FacetOrientation.North:
                    return false;
                case FacetOrientation.South:
                    return true;
                case FacetOrientation.Top:
                    return false;
                case FacetOrientation.Bottom:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
            }
        }

    }
}
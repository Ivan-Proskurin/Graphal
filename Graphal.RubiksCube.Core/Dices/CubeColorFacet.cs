using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using Graphal.Engine.ThreeD.Geometry;
using Graphal.Engine.ThreeD.Primitives;

namespace Graphal.RubiksCube.Core.Dices
{
    public class CubeColorFacet : IEnumerable<Triangle3D>
    {
        private readonly int _width;
        private readonly bool _invert;
        private readonly int _semiwidth;
        private readonly List<Triangle3D> _triangles = new List<Triangle3D>();

        public CubeColorFacet(int width, Vector3D position, FacetDimensions dimensions, Color? color, bool invert)
        {
            _width = width;
            _invert = invert;
            _semiwidth = width / 2;
            CreateTriangles(position, dimensions, color);
        }

        public IEnumerator<Triangle3D> GetEnumerator()
        {
            return _triangles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void CreateTriangles(Vector3D position, FacetDimensions dimensions, Color? color)
        {
            switch (dimensions)
            {
                case FacetDimensions.YZ:
                    if (color != null)
                    {
                        CreateYZColouredTriangles(position, color.Value);
                    }
                    else
                    {
                        CreateYZSolidTriangles(position);
                    }
                    break;
                case FacetDimensions.XZ:
                    if (color != null)
                    {
                        CreateXZColouredTriangles(position, color.Value);
                    }
                    else
                    {
                        CreateXZSolidTriangles(position);
                    }
                    break;
                case FacetDimensions.XY:
                    if (color != null)
                    {
                        CreateXYColouredTriangles(position, color.Value);
                    }
                    else
                    {
                        CreateXYSolidTriangles(position);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dimensions), dimensions, null);
            }
        }

        private void CreateXZColouredTriangles(Vector3D position, Color color)
        {
            var background = RubikCube.BackgroundColor;
            var x = position.X;
            var y = position.Y;
            var z = position.Z;
            var z0 = z + _semiwidth;
            var x0 = x - _semiwidth;
            var zn = z - _semiwidth;
            var xn = x + _semiwidth;
            var dx = _width / 10;
            var dz = _width / 10;

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (i > 0 && i < 9 && j > 0 && j < 9)
                    {
                        continue;
                    }

                    var v11 = new Vector3D(x0 + dx * i, y, z0 - dz * (j + 1));
                    var v12 = new Vector3D(x0 + dx * i, y, z0 - dz * j);
                    var v13 = new Vector3D(x0 + dx * (i + 1), y, z0 - dz * j);
                
                    var v21 = new Vector3D(x0 + dx * i, y, z0 - dz * (j + 1));
                    var v22 = new Vector3D(x0 + dx * (i + 1), y, z0 - dz * j);
                    var v23 = new Vector3D(x0 + dx * (i + 1), y, z0 - dz * (j + 1));

                    if (_invert)
                    {
                        Vector3D.Swap(ref v11, ref v13);
                        Vector3D.Swap(ref v21, ref v23);
                    }
                    
                    var triangle1 = new Triangle3D(v11, v12, v13, background);
                    var triangle2 = new Triangle3D(v21, v22, v23, background);
                    _triangles.Add(triangle1);
                    _triangles.Add(triangle2);
                }
            }

            var vc11 = new Vector3D(x0 + dx, y, z0 - dz);
            var vc12 = new Vector3D(xn - dx, y, z0 - dz);
            var vc13 = new Vector3D(x0 + dx, y, zn + dz);
            
            var vc21 = new Vector3D(xn - dx, y, z0 - dz);
            var vc22 = new Vector3D(xn - dx, y, zn + dz);
            var vc23 = new Vector3D(x0 + dx, y, zn + dz);

            if (_invert)
            {
                Vector3D.Swap(ref vc11, ref vc13);
                Vector3D.Swap(ref vc21, ref vc23);
            }
            
            var colorTriangle1 = new Triangle3D(vc11, vc12, vc13, color);
            _triangles.Add(colorTriangle1);
            var colorTriangle2 = new Triangle3D(vc21, vc22, vc23, color);
            _triangles.Add(colorTriangle2);
            
        }

        private void CreateXZSolidTriangles(Vector3D position)
        {
            var background = RubikCube.BackgroundColor;
            var x = position.X;
            var y = position.Y;
            var z = position.Z;
            var z0 = z + _semiwidth;
            var zn = z - _semiwidth;
            var x0 = x - _semiwidth;
            var xn = x + _semiwidth;

            var v11 = new Vector3D(x0, y, z0);
            var v12 = new Vector3D(xn, y, z0);
            var v13 = new Vector3D(x0, y, zn);
        
            var v21 = new Vector3D(xn, y, z0);
            var v22 = new Vector3D(xn, y, zn);
            var v23 = new Vector3D(x0, y, zn);

            if (_invert)
            {
                Vector3D.Swap(ref v11, ref v13);
                Vector3D.Swap(ref v21, ref v23);
            }
            
            var triangle1 = new Triangle3D(v11, v12, v13, background);
            _triangles.Add(triangle1);
            var triangle2 = new Triangle3D(v21, v22, v23, background);
            _triangles.Add(triangle2);

        }

        private void CreateXYColouredTriangles(Vector3D position, Color color)
        {
            var background = RubikCube.BackgroundColor;
            var x = position.X;
            var y = position.Y;
            var z = position.Z;
            var dx = _width / 10;
            var dy = _width / 10;
            var x0 = x - _semiwidth;
            var y0 = y + _semiwidth;
            var xn = x + _semiwidth;
            var yn = y - _semiwidth;

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (i > 0 && i < 9 && j > 0 && j < 9)
                    {
                        continue;
                    }

                    var v11 = new Vector3D(x0 + dx * i, y0 - dy * (j + 1), z);
                    var v12 = new Vector3D(x0 + dx * i, y0 - dy * j, z);
                    var v13 = new Vector3D(x0 + dx * (i + 1), y0 - dy * j, z);
                
                    var v21 = new Vector3D(x0 + dx * i, y0 - dy * (j + 1), z);
                    var v22 = new Vector3D(x0 + dx * (i + 1), y0 - dy * j, z);
                    var v23 = new Vector3D(x0 + dx * (i + 1), y0 - dy * (j + 1), z);

                    if (_invert)
                    {
                        Vector3D.Swap(ref v11, ref v13);
                        Vector3D.Swap(ref v21, ref v23);
                    }
                    
                    var triangle1 = new Triangle3D(v11, v12, v13, background);
                    var triangle2 = new Triangle3D(v21, v22, v23, background);
                    _triangles.Add(triangle1);
                    _triangles.Add(triangle2);
            
                }
            }

            var vc11 = new Vector3D(x0 + dx, y0 - dy, z);
            var vc12 = new Vector3D(xn - dx, y0 - dy, z);
            var vc13 = new Vector3D(x0 + dx, yn + dy, z);

            var vc21 = new Vector3D(xn - dx, y0 - dy, z);
            var vc22 = new Vector3D(xn - dx, yn + dy, z);
            var vc23 = new Vector3D(x0 + dx, yn + dy, z);

            if (_invert)
            {
                Vector3D.Swap(ref vc11, ref vc13);
                Vector3D.Swap(ref vc21, ref vc23);
            }
            
            var colorTriangle1 = new Triangle3D(vc11, vc12, vc13, color);
            _triangles.Add(colorTriangle1);
            var colorTriangle2 = new Triangle3D(vc21, vc22, vc23, color);
            _triangles.Add(colorTriangle2);

        }

        private void CreateXYSolidTriangles(Vector3D position)
        {
            var background = RubikCube.BackgroundColor;
            var x = position.X;
            var y = position.Y;
            var z = position.Z;
            var x0 = x - _semiwidth;
            var y0 = y + _semiwidth;
            var xn = x + _semiwidth;
            var yn = y - _semiwidth;

            var v11 = new Vector3D(x0, y0, z);
            var v12 = new Vector3D(xn, y0, z);
            var v13 = new Vector3D(x0, yn, z);
        
            var v21 = new Vector3D(xn, y0, z);
            var v22 = new Vector3D(xn, yn, z);
            var v23 = new Vector3D(x0, yn, z);

            if (_invert)
            {
                Vector3D.Swap(ref v11, ref v13);
                Vector3D.Swap(ref v21, ref v23);
            }
            
            var triangle1 = new Triangle3D(v11, v12, v13, background);
            _triangles.Add(triangle1);

            var triangle2 = new Triangle3D(v21, v22, v23, background);
            _triangles.Add(triangle2);

        }

        private void CreateYZColouredTriangles(Vector3D position, Color color)
        {
            var background = RubikCube.BackgroundColor;
            var x = position.X;
            var y = position.Y;
            var z = position.Z;
            var y0 = y - _semiwidth;
            var z0 = z + _semiwidth;
            var yn = y + _semiwidth;
            var zn = z - _semiwidth;
            var dy = _width / 10;
            var dz = _width / 10;

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    if (i > 0 && i < 9 && j > 0 && j < 9)
                    {
                        continue;
                    }

                    var v11 = new Vector3D(x, y0 + dy * i, z0 - dz * (j + 1));
                    var v12 = new Vector3D(x, y0 + dy * i, z0 - dz * j);
                    var v13 = new Vector3D(x, y0 + dy * (i + 1), z0 - dz * j);
                
                    var v21 = new Vector3D(x, y0 + dy * i, z0 - dz * (j + 1));
                    var v22 = new Vector3D(x, y0 + dy * (i + 1), z0 - dz * j);
                    var v23 = new Vector3D(x, y0 + dy * (i + 1), z0 - dz * (j + 1));

                    if (_invert)
                    {
                        Vector3D.Swap(ref v11, ref v13);
                        Vector3D.Swap(ref v21, ref v23);
                    }
                
                    var triangle1 = new Triangle3D(v11, v12, v13, background);
                    var triangle2 = new Triangle3D(v21, v22, v23, background);
                    _triangles.Add(triangle1);
                    _triangles.Add(triangle2);
                }
            }

            var vc11 = new Vector3D(x, y0 + dy, z0 - dz);
            var vc12 = new Vector3D(x, yn - dy, z0 - dz);
            var vc13 = new Vector3D(x, y0 + dy, zn + dz);
            
            var vc21 = new Vector3D(x, yn - dy, z0 - dz);
            var vc22 = new Vector3D(x, yn - dy, zn + dz);
            var vc23 = new Vector3D(x, y0 + dy, zn + dz);

            if (_invert)
            {
                Vector3D.Swap(ref vc11, ref vc13);
                Vector3D.Swap(ref vc21, ref vc23);
            }
            
            var colorTriangle1 = new Triangle3D(vc11, vc12, vc13, color);
            _triangles.Add(colorTriangle1);
            
            var colorTriangle2 = new Triangle3D(vc21, vc22, vc23, color);
            _triangles.Add(colorTriangle2);
        }

        private void CreateYZSolidTriangles(Vector3D position)
        {
            var background = RubikCube.BackgroundColor;
            var x = position.X;
            var y = position.Y;
            var z = position.Z;
            var y0 = y - _semiwidth;
            var z0 = z + _semiwidth;
            var yn = y + _semiwidth;
            var zn = z - _semiwidth;

            var v11 = new Vector3D(x, y0, z0);
            var v12 = new Vector3D(x, yn, z0);
            var v13 = new Vector3D(x, y0, zn);
            
            var v21 = new Vector3D(x, yn, z0);
            var v22 = new Vector3D(x, yn, zn);
            var v23 = new Vector3D(x, y0, zn);

            if (_invert)
            {
                Vector3D.Swap(ref v11, ref v13);
                Vector3D.Swap(ref v21, ref v23);
            }
            
            var triangle1 = new Triangle3D(v11, v12, v13, background);
            _triangles.Add(triangle1);
            
            var triangle2 = new Triangle3D(v21, v22, v23, background);
            _triangles.Add(triangle2);

        }
    }
}
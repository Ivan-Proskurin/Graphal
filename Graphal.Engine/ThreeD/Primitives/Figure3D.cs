using System.Collections;
using System.Collections.Generic;

namespace Graphal.Engine.ThreeD.Primitives
{
    public class Figure3D : IEnumerable<Triangle3D>
    {
        private readonly List<Triangle3D> _triangles = new List<Triangle3D>();

        public void Append(Triangle3D triangle)
        {
            _triangles.Add(triangle);
        }

        public void Remove(Triangle3D triangle)
        {
            _triangles.Remove(triangle);
        }

        public IEnumerator<Triangle3D> GetEnumerator()
        {
            return _triangles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
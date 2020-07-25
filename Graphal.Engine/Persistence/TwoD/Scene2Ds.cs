using System.Collections.Generic;

using Graphal.Engine.Persistence.TwoD.Primitives;
using Graphal.Engine.Persistence.TwoD.Transforms;

namespace Graphal.Engine.Persistence.TwoD
{
    public class Scene2Ds
    {
        public Transform2Ds Transform { get; set; }

        public List<Primitive2Ds> Primitives { get; set; }
    }
}
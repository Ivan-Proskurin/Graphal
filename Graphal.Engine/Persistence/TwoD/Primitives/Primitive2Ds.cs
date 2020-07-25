using System.Xml.Serialization;

using Graphal.Engine.TwoD.Primitives;

namespace Graphal.Engine.Persistence.TwoD.Primitives
{
    [XmlInclude(typeof(Point2Ds))]
    [XmlInclude(typeof(Edge2Ds))]
    [XmlInclude(typeof(Triangle2Ds))]
    public abstract class Primitive2Ds
    {
        public abstract Primitive2D ToPrimitive2D();
    }
}
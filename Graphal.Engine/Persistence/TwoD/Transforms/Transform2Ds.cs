using System.Xml.Serialization;

using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.Persistence.TwoD.Transforms
{
    [XmlInclude(typeof(ShiftTransform2Ds))]
    public abstract class Transform2Ds
    {
        public abstract Transform2D ToTransform2D();
    }
}
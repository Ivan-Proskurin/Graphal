using System.Linq;

namespace Graphal.RubiksCube.Core.Extensions
{
    public static class CubePositionExtensions
    {
        public static CubeDimension GetPosition(this CubeDimension[] dimensions)
        {
            return dimensions.Aggregate(
                CubeDimension.None, 
                (result, current) =>
                {
                    result |= current;
                    return result;
                });
        }
        
        public static bool IsInDimension(this CubeDimension position, CubeDimension dimension)
        {
            return (position & dimension) != CubeDimension.None;
        }

        public static bool Contains(this CubeDimension position, CubeDimension dimension)
        {
            return (position & dimension) != CubeDimension.None;
        }

        public static CubeDimension Subtract(this CubeDimension dimension, CubeDimension dimension1)
        {
            return dimension ^ dimension1;
        }
    }
}
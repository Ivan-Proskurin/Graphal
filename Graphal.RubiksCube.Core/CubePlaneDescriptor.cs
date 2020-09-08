using System.Collections.Generic;

using Graphal.RubiksCube.Core.Dices;

namespace Graphal.RubiksCube.Core
{
    public class CubePlaneDescriptor
    {
        public CubePlane Plane { get; set; }
        
        public PlaneRotationInfo RotationInfo { get; set; }
        
        public List<RubiksDice> Dices { get; set; }
    }
}
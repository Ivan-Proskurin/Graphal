using Graphal.Engine.Abstractions.ThreeD.Animation;

namespace Graphal.Engine.ThreeD.Animation
{
    public class ModelRotationAnimation : AnimationInfo
    {
        public ModelRotationAnimation(RotationPhase phase, double radiansXZ, double radiansYZ)
        {
            Phase = phase;
            Code = (int) AnimationCodeTypes.ObjectRotation;
            RadiansXZ = radiansXZ;
            RadiansYZ = radiansYZ;
            IsMandatory = phase == RotationPhase.Start || phase == RotationPhase.Stop;
        }
        
        public RotationPhase Phase { get; }
        
        public double RadiansXZ { get; }
        
        public double RadiansYZ { get; }
    }

    public enum RotationPhase
    {
        Start,
        Rotate,
        Stop,
    }
}
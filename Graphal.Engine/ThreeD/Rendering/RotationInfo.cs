namespace Graphal.Engine.ThreeD.Rendering
{
    public class RotationInfo
    {
        public RotationType RotationType { get; set; }
        public double RotateRadiansTotals { get; set; }
    }
    
    public enum RotationType
    {
       RotationXZ,
       RotationYZ,
    }
}
using Graphal.Engine.Abstractions.ThreeD.Animation;

namespace Graphal.Engine.ThreeD.Animation
{
    public class MoveCloserAnimation : AnimationInfo
    {
        public MoveCloserAnimation(double grade)
        {
            Code = (int) AnimationCodeTypes.MoveCloser;
            Grade = grade;
        }

        public double Grade { get; }
    }
}
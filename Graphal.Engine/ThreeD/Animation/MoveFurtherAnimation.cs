using Graphal.Engine.Abstractions.ThreeD.Animation;

namespace Graphal.Engine.ThreeD.Animation
{
    public class MoveFurtherAnimation : AnimationInfo
    {
        public MoveFurtherAnimation(double grade)
        {
            Code = (int) AnimationCodeTypes.MoveFurther;
            Grade = grade;
        }
        
        public double Grade { get; }
    }
}
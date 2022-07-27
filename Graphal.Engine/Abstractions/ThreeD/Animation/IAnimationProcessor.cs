using System;
using System.Threading.Tasks;

namespace Graphal.Engine.Abstractions.ThreeD.Animation
{
    public interface IAnimationProcessor
    {
        void EnqueueAnimation(AnimationInfo animationInfo);

        public event ProcessAnimationEventHandler ProcessAnimation;
    }

    public class ProcessAnimationEventArgs : EventArgs
    {
        public ProcessAnimationEventArgs(object sender, AnimationInfo animationInfo)
        {
            Sender = sender;
            AnimationInfo = animationInfo;
        }

        public object Sender { get; }
        
        public AnimationInfo AnimationInfo { get; }
    }
    
    public delegate Task ProcessAnimationEventHandler(ProcessAnimationEventArgs e);
}
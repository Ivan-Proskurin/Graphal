using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Graphal.Engine.Abstractions.Logging;
using Graphal.Engine.Abstractions.ThreeD.Animation;

namespace Graphal.Engine.ThreeD.Animation
{
    public class AnimationProcessor : IAnimationProcessor
    {
        private readonly ILogger _logger;
        private readonly ConcurrentQueue<AnimationInfo> _animationQueue = new ConcurrentQueue<AnimationInfo>();
        private Task _processAnimationTask;

        public AnimationProcessor(ILogger logger)
        {
            _logger = logger;
        }
        
        public void EnqueueAnimation(AnimationInfo animationInfo)
        {
            _animationQueue.Enqueue(animationInfo);
            TryStartAnimationProcessing();
        }

        private void TryStartAnimationProcessing()
        {
            if (_processAnimationTask != null)
            {
                return;
            }

            _processAnimationTask = Task.Run(async () => await ProcessAnimationInfos());
        }

        private async Task OnProcessAnimation(AnimationInfo animationInfo)
        {
            if (ProcessAnimation != null)
            {
                await ProcessAnimation(new ProcessAnimationEventArgs(this, animationInfo));
            }
        }

        public event ProcessAnimationEventHandler ProcessAnimation;

        private async Task ProcessAnimationInfos()
        {
            var stopwatch = new Stopwatch();
            var framesCount = 0;
            try
            {
                while (true)
                {
                    var animationInfos = TryDequeueLastAnimationInfo();
                    if (animationInfos == null || !animationInfos.Any())
                    {
                        if (stopwatch.IsRunning)
                        {
                            stopwatch.Stop();
                            var fps = Convert.ToDouble(framesCount * 1000 / stopwatch.ElapsedMilliseconds);
                            _logger.Info($"Last FPS: {Math.Round(fps)}");
                        }
                        await Task.Delay(100);
                        continue;
                    }

                    if (!stopwatch.IsRunning)
                    {
                        stopwatch.Start();
                    }

                    foreach (var animationInfo in animationInfos)
                    {
                        await OnProcessAnimation(animationInfo);
                        framesCount++;
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.Info($"Animation processing error: {exception.Message}");
            }
        }

        private AnimationInfo[] TryDequeueLastAnimationInfo()
        {
            var animations = new Dictionary<int, AnimationInfo>();
            AnimationInfo animationInfo;
            while (_animationQueue.TryDequeue(out var result))
            {
                animationInfo = result;
                animations[animationInfo.Code] = animationInfo;
                if (animationInfo.IsMandatory)
                {
                    break;
                }
            }

            return animations.Values.ToArray();
        }
    }
}
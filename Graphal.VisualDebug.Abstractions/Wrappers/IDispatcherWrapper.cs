using System;

namespace Graphal.VisualDebug.Abstractions.Wrappers
{
    public interface IDispatcherWrapper
    {
        void Invoke(Action action);
    }
}
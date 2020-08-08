using System;
using System.Windows.Threading;

using Graphal.VisualDebug.Abstractions.Wrappers;

namespace Graphal.VisualDebug.Wrappers
{
    public class DispatcherWrapper : IDispatcherWrapper
    {
        private readonly Dispatcher _dispatcher;

        public DispatcherWrapper(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            _dispatcher.Invoke(action);
        }
    }
}
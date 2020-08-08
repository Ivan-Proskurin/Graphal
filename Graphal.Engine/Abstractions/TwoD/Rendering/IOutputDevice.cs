using System.Drawing;

namespace Graphal.Engine.Abstractions.TwoD.Rendering
{
    public interface IOutputDevice
    {
        void Lock();
        
        void MoveBuffer(int[] buffer);

        void AddDirtyRect(Rectangle dirtyRect);

        void Unlock();
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Graphal.Engine.Persistence.TwoD;
using Graphal.Engine.TwoD.Geometry;
using Graphal.Engine.TwoD.Primitives;
using Graphal.Engine.TwoD.Transforms;

namespace Graphal.Engine.Abstractions.TwoD.Rendering
{
    public interface IScene2D
    {
        void Append(Primitive2D primitive);

        void SetShift(Vector2D shift);

        Task RenderAsync();

        Task BeginShiftAsync(int x, int y);

        Task<ShiftTransform2D> ShiftAsync(int x, int y);

        Task EndShiftAsync(int x, int y);

        Vector2D ToWorldCoordinates(Vector2D v);

        Scene2Ds ToScene2Ds();

        void FromScene2Ds(Scene2Ds container);

        void FromProjection(IEnumerable<Primitive2D> triangles);

        event FpsChangedEventHandler FpsChanged;
    }

    public class FpsChangedArgs : EventArgs
    {
        public FpsChangedArgs(int fps)
        {
            Fps = fps;
        }
        public int Fps { get; }
    }
    
    public delegate void FpsChangedEventHandler(object sender, FpsChangedArgs e);
}
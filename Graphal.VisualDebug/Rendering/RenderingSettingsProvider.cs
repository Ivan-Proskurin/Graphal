using Graphal.Engine.Abstractions.TwoD.Rendering;
using Graphal.Engine.TwoD.Rendering;

namespace Graphal.VisualDebug.Rendering
{
    public class RenderingSettingsProvider : IRenderingSettingsProvider
    {
        public RenderingSettings GetRenderingSettings()
        {
            return new RenderingSettings
            {
                ScreenWidth = 1980,
                ScreenHeight = 1024,
            };
        }
    }
}
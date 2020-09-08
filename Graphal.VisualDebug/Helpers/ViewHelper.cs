using System.Windows;
using System.Windows.Input;

namespace Graphal.VisualDebug.Helpers
{
    public static class ViewHelper
    {
        public static T GetViewModel<T>(this FrameworkElement frameworkElement) where T : class
        {
            return frameworkElement.DataContext as T;
        }

        public static (int x, int y) GetClickPoint(this MouseButtonEventArgs e, IInputElement relativeTo)
        {
            var clickPoint = e.GetPosition(relativeTo);
            return ((int) clickPoint.X, (int) clickPoint.Y);
        }

        public static (int x, int y) GetClickPoint(this MouseEventArgs e, IInputElement relativeTo)
        {
            var clickPoint = e.GetPosition(relativeTo);
            return ((int) clickPoint.X, (int) clickPoint.Y);
        }
    }
}
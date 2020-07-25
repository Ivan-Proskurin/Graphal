using System.Windows;

namespace Graphal.VisualDebug.Helpers
{
    public static class DataContextHelper
    {
        public static T GetViewModel<T>(this FrameworkElement frameworkElement) where T : class
        {
            return frameworkElement.DataContext as T;
        }
    }
}
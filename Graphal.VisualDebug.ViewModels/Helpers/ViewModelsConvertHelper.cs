using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Graphal.VisualDebug.ViewModels.Helpers
{
    internal static class ViewModelsConvertHelper
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> value)
        {
            return new ObservableCollection<T>(value);
        }
    }
}
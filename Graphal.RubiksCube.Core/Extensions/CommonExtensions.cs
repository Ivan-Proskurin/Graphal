using System;
using System.Collections.Generic;

namespace Graphal.RubiksCube.Core.Extensions
{
    public static class CommonExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> elements, Action<T> action)
        {
            foreach (var element in elements)
            {
                action(element);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Problem.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ContainsSafe<T>(this IEnumerable<T> input, T value)
        {
            if (input == null)
            {
                return false;
            }

            return input.Contains(value);
        }
    }
}
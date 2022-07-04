using System;

namespace PractiveWork3.Extensions
{
    public static class BasicExtensions
    {
        public static bool IsPositive<T>(this T value) where T : IComparable<T>
        {
            return value.CompareTo(default(T)) > 0;
        }
        public static bool IsNegative<T>(this T value) where T : IComparable<T>
        {
            return value.CompareTo(default(T)) <= 0;
        }
    }
}

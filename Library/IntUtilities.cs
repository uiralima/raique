using System;

namespace Raique.Library
{
    public static class IntUtilities
    {
        public static void ThrowIfZero(this int target, string objectName)
        {
            if (0 == target)
            {
                throw new ArgumentException(objectName);
            }
        }

        public static bool IsGreaterThanZero(this int target)
        {
            return target.IsGreaterThan(0);
        }

        public static bool IsGreaterThan(this int target, int value)
        {
            return target > value;
        }

        public static bool IsLessThanZero(this int target)
        {
            return target.IsLessThan(0);
        }

        public static bool IsLessThan(this int target, int value)
        {
            return target < value;
        }

        public static bool IsGreaterThanOrEqualZero(this int target)
        {
            return target.IsGreaterThanOrEqual(0);
        }

        public static bool IsGreaterThanOrEqual(this int target, int value)
        {
            return target >= value;
        }

        public static bool IsLessThanOrEqualZero(this int target)
        {
            return target.IsLessThanOrEqual(0);
        }

        public static bool IsLessThanOrEqual(this int target, int value)
        {
            return target <= value;
        }
    }
}

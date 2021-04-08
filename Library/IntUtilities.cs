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
    }
}

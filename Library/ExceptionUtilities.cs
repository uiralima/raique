using System;

namespace Raique.Library
{
    public delegate Exception Creator();
    public static class ExceptionUtilities
    {
        public static void ThrowIfFalse(this Func<Exception> creator, bool target) => creator.ThrowIfTrue(!target);
        public static void ThrowIfTrue(this Func<Exception> creator, bool target)
        {
            if (target)
                throw creator();
        }
    }
}

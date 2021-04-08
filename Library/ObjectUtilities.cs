using System;

namespace Raique.Library
{
    public static class ObjectUtilities
    {
        public static void ThrowIfNull(this object target, string objectName)
        {
            if (null == target)
            {
                throw new ArgumentNullException(objectName);
            }
        }
    }
}

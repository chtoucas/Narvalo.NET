// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public static class ConvertTo
    {
        /// <remarks>
        /// WARNING: Does not work consistently for Flags enums:
        /// http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx
        /// </remarks>
        public static TEnum? Enum<TEnum>(object value) where TEnum : struct
        {
            var type = typeof(TEnum);

            if (System.Enum.IsDefined(type, value))
            {
                return (TEnum)System.Enum.ToObject(type, value);
            }
            else
            {
                return null;
            }
        }
    }
}

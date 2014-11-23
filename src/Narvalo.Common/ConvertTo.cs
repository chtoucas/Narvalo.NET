// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    public static class ConvertTo
    {
        /// <remarks>
        /// Does not work consistently for Flags enums:
        /// http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx
        /// </remarks>
        public static TEnum? Enum<TEnum>(object value) where TEnum : struct
        {
            MoreCheck.IsEnum(typeof(TEnum));

            if (System.Enum.IsDefined(typeof(TEnum), value)) {
                return (TEnum)System.Enum.ToObject(typeof(TEnum), value);
            }
            else {
                return null;
            }
        }
    }
}

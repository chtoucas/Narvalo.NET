// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public static class ConvertTo
    {
        /// <remarks>
        /// Does not work consistently for Flags enums:
        /// http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx
        /// </remarks>
        public static TEnum? Enum<TEnum>(object value) where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum) {
                throw new InvalidOperationException(Format.CurrentCulture(SR.TypeIsNotEnumFormat, type.FullName ?? "Unknown type name"));
            }

            if (System.Enum.IsDefined(type, value)) {
                return (TEnum)System.Enum.ToObject(type, value);
            }
            else {
                return null;
            }
        }
    }
}

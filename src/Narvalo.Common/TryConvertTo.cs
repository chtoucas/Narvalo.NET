// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Diagnostics.CodeAnalysis;

    public static partial class TryConvertTo
    {
        /// <remarks>
        /// Does not work consstently with Flags enums:
        /// http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool Enum<TEnum>(object value, out TEnum result) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            result = default(TEnum);

            if (System.Enum.IsDefined(typeof(TEnum), value)) {
                result = (TEnum)System.Enum.ToObject(typeof(TEnum), value);
                return true;
            }
            else {
                return false;
            }
        }
    }
}

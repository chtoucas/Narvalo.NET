// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Narvalo.Linq;

    public static class TryParseTo
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "The method already returns a boolean to indicate the result.")]
        public static bool Boolean(string value, out bool result)
        {
            return Boolean(value, BooleanStyles.Default, out result);
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "The method already returns a boolean to indicate the result.")]
        public static bool Boolean(string value, BooleanStyles style, out bool result)
        {
            result = default(Boolean);

            if (value == null) { return false; }

            var val = value.Trim();

            if (val.Length == 0) {
                if (style.HasFlag(BooleanStyles.EmptyIsFalse)) {
                    result = false;
                    return true;
                }
                else {
                    return false;
                }
            }

            if (style.HasFlag(BooleanStyles.Literal)) {
                // NB: Cette méthode n'est pas sensible à la casse de "value".
                if (System.Boolean.TryParse(val, out result)) {
                    return true;
                }
            }

            if (style.HasFlag(BooleanStyles.Integer)) {
                int? intValue = from _ in ParseTo.NullableInt32(val, NumberStyles.Integer, CultureInfo.InvariantCulture)
                                where _ == 0 || _ == 1
                                select _;

                if (intValue.HasValue) {
                    result = intValue.Value == 1;
                    return true;
                }
            }

            if (style.HasFlag(BooleanStyles.HtmlInput)) {
                return value == "on";
            }

            return false;
        }

#if NET_35

        /// <example>
        /// <code>
        /// MyEnum result;
        /// TryParseTo.Enum&lt;MyEnum&gt;("ActualValue", out result);
        /// </code>
        /// </example>
        public static bool Enum<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return Enum<TEnum>(value, false /* ignoreCase */, out result);
        }

        /// <example>
        /// <code>
        /// MyEnum result;
        /// TryParseTo.Enum&lt;MyEnum&gt;("actualvalue", out result, true /* ignoreCase */);
        /// </code>
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "La méthode retourne un booléen pour indiquer le succès ou l'échec.")]
        public static bool Enum<TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct
        {
            // FIXME: ne marche pas de manière cohérente pour les enum's de type Flags.
            DebugCheck.IsEnum(typeof(TEnum));

            //if (Attribute.IsDefined(type, typeof(FlagsAttribute))) {
            //  throw Failure.Argument();
            //}

            result = default(TEnum);

            if (String.IsNullOrEmpty(value)) {
                return false;
            }

            StringComparison comparison = ignoreCase
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture;

            foreach (string name in System.Enum.GetNames(typeof(TEnum))) {
                if (name.Equals(value, comparison)) {
                    result = (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);

                    return true;
                }
            }

            return false;
        }

#endif
    }
}

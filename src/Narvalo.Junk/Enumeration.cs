// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
#if NET_35
    using System;
#endif
    using System.Diagnostics.CodeAnalysis;

    public static class Enumeration
    {
        /// <remarks>
        /// Does not work consistently for Flags enums:
        /// http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#",
            Justification = "The method already returns a boolean to indicate the outcome.")]
        public static bool TryConvert<TEnum>(object value, out TEnum result) where TEnum : struct
        {
            MoreCheck.IsEnum(typeof(TEnum));

            result = default(TEnum);

            if (System.Enum.IsDefined(typeof(TEnum), value)) {
                result = (TEnum)System.Enum.ToObject(typeof(TEnum), value);
                return true;
            }
            else {
                return false;
            }
        }

        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            return Parse<TEnum>(value, ignoreCase: false);
        }

        public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            MoreCheck.IsEnum(typeof(TEnum));

            return (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

#if NET_35

        /// <example>
        /// <code>
        /// MyEnum result;
        /// Enumeration.TryParse&lt;MyEnum&gt;("ActualValue", out result);
        /// </code>
        /// </example>
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return TryParse<TEnum>(value, false /* ignoreCase */, out result);
        }

        /// <example>
        /// <code>
        /// MyEnum result;
        /// Enumeration.TryParse&lt;MyEnum&gt;("actualvalue", out result, true /* ignoreCase */);
        /// </code>
        /// </example>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", 
            Justification = "The method already returns a boolean to indicate the outcome.")]
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct
        {
            // FIXME: ne marche pas de manière cohérente pour les enum's de type Flags.
            Check.IsEnum(typeof(TEnum));

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

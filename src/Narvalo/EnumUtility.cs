namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Diagnostics;
    using Narvalo.Fx;

    /// <example>
    /// <code>
    /// public enum MyEnum {
    ///		Default = 0,
    ///		ActualValue = 1,
    ///	}
    /// </code>
    /// </example>
    public static class EnumUtility
    {
        #region > Utilitaires de conversion <

        /// <example>
        /// <code>
        ///	Maybe&lt;MyEnum&gt; value = EnumUtility.MayConvert&lt;MyEnum&gt;(1);
        /// </code>
        /// </example>
        public static Maybe<TEnum> MayConvert<TEnum>(object value) where TEnum : struct
        {
            TEnum result;
            if (!TryConvert<TEnum>(value, out result)) {
                return Maybe<TEnum>.None;
            }
            return Maybe.Create(result);
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public static bool TryConvert<TEnum>(object value, out TEnum result) where TEnum : struct
        {
            // FIXME: ne marche pas de manière cohérente pour les enum's de type Flags.
            // Cf. http://msdn.microsoft.com/en-us/library/system.enum.isdefined.aspx

            __Asserts.IsEnum(typeof(TEnum));

            result = default(TEnum);

            if (Enum.IsDefined(typeof(TEnum), value)) {
                result = (TEnum)Enum.ToObject(typeof(TEnum), value);
                return true;
            }
            else {
                return false;
            }
        }

        #endregion

        #region > Utilitaires d'analyse <

        /// <example>
        /// <code>
        ///	MyEnum value = EnumUtility.Parse&lt;MyEnum&gt;("ActualValue");
        /// </code>
        /// </example>
        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            return Parse<TEnum>(value, false /* ignoreCase */);
        }

        /// <example>
        /// <code>
        ///	MyEnum value = EnumUtility.Parse&lt;MyEnum&gt;("actualvalue", true /* ignoreCase */);
        /// </code>
        /// </example>    	
        public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            __Asserts.IsEnum(typeof(TEnum));

            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }

#if NET_35

        /// <example>
        /// <code>
        /// MyEnum result;
        /// EnumUtility.TryParse&lt;MyEnum&gt;("ActualValue", out result);
        /// </code>
        /// </example>
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return TryParse<TEnum>(value, false /* ignoreCase */, out result);
        }

        /// <example>
        /// <code>
        /// MyEnum result;
        /// EnumUtility.TryParse&lt;MyEnum&gt;("actualvalue", out result, true /* ignoreCase */);
        /// </code>
        /// </example>
        
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#",
            Justification = "The method already returns a boolean to indicate failure or success.")]
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct
        {
            // FIXME: ne marche pas de manière cohérente pour les enum's de type Flags.

            __Asserts.IsEnum(typeof(TEnum));

            //if (Attribute.IsDefined(type, typeof(FlagsAttribute))) {
            //  throw Failure.ArgumentException();
            //}

            result = default(TEnum);

            if (String.IsNullOrEmpty(value)) {
                return false;
            }

            StringComparison comparison = ignoreCase
                ? StringComparison.InvariantCultureIgnoreCase
                : StringComparison.InvariantCulture;

            foreach (string name in Enum.GetNames(typeof(TEnum))) {
                if (name.Equals(value, comparison)) {
                    result = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);

                    return true;
                }
            }

            return false;
        }

#endif

        #endregion
    }
}

namespace Narvalo
{
    using System;

    public static class Parse
    {
        /// <example>
        /// <code>
        /// MyEnum value = Parse.ToEnum&lt;MyEnum&gt;("ActualValue");
        /// </code>
        /// </example>
        public static TEnum ToEnum<TEnum>(string value) where TEnum : struct
        {
            return ToEnum<TEnum>(value, false /* ignoreCase */);
        }

        /// <example>
        /// <code>
        /// MyEnum value = Parse.ToEnum&lt;MyEnum&gt;("actualvalue", true /* ignoreCase */);
        /// </code>
        /// </example>
        public static TEnum ToEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
    }
}

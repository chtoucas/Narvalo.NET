namespace Narvalo
{
    using System;

    public static class Parse
    {
        public static TEnum ToEnum<TEnum>(string value) where TEnum : struct
        {
            return ToEnum<TEnum>(value, false /* ignoreCase */);
        }

        public static TEnum ToEnum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
    }
}

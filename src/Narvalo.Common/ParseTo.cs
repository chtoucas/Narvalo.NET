namespace Narvalo
{
    public static class ParseTo
    {
        public static TEnum Enum<TEnum>(string value) where TEnum : struct
        {
            return Enum<TEnum>(value, false /* ignoreCase */);
        }

        public static TEnum Enum<TEnum>(string value, bool ignoreCase) where TEnum : struct
        {
            DebugCheck.IsEnum(typeof(TEnum));

            return (TEnum)System.Enum.Parse(typeof(TEnum), value, ignoreCase);
        }
    }
}

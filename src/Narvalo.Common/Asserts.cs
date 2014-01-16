namespace Narvalo
{
    using System;
    using System.Diagnostics;

    public static class Asserts
    {
        [Conditional("DEBUG")]
        public static void IsEnum(Type type)
        {
            Requires.NotNull(type, "type");

            Debug.Assert(type.IsEnum);
        }

        [Conditional("DEBUG")]
        public static void IsValueType(Type type)
        {
            Requires.NotNull(type, "type");

            Debug.Assert(type.IsValueType);
        }
    }
}

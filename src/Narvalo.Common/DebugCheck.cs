namespace Narvalo
{
    using System;
    using System.Diagnostics;

    public static class DebugCheck
    {
        [Conditional("DEBUG")]
        public static void IsEnum(Type type)
        {
            Require.NotNull(type, "type");

            Debug.Assert(type.IsEnum, type.FullName, SR.DebugCheck_TypeIsNotEnum);
        }

        [Conditional("DEBUG")]
        public static void IsValueType(Type type)
        {
            Require.NotNull(type, "type");

            Debug.Assert(type.IsValueType, type.FullName, SR.DebugCheck_TypeIsNotValueType);
        }
    }
}

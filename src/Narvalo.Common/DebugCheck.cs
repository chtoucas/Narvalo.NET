namespace Narvalo
{
    using System;
    using System.Diagnostics;

    public static class DebugCheck
    {
        [Conditional("DEBUG")]
        public static void NotNull<T>(T value)
        {
            Debug.Assert(value != null, SR.DebugCheck_IsNull);
        }

        [Conditional("DEBUG")]
        public static void NotNullOrEmpty(string value)
        {
            NotNull(value);
            Debug.Assert(value.Length != 0, SR.DebugCheck_IsEmpty);
        }

        [Conditional("DEBUG")]
        public static void IsEnum(Type type)
        {
            NotNull(type);
            Debug.Assert(type.IsEnum, type.FullName, SR.DebugCheck_TypeIsNotEnum);
        }

        [Conditional("DEBUG")]
        public static void IsValueType(Type type)
        {
            NotNull(type);
            Debug.Assert(type.IsValueType, type.FullName, SR.DebugCheck_TypeIsNotValueType);
        }
    }
}

namespace Narvalo
{
    using System;
    using System.Diagnostics;

    public static class Check
    {
        [Conditional("DEBUG")]
        public static void IsEnum(Type type)
        {
            Require.NotNull(type, "type");

            Debug.Assert(type.IsEnum, SR.Check_TypeMustBeEnum);
        }

        [Conditional("DEBUG")]
        public static void IsValueType(Type type)
        {
            Require.NotNull(type, "type");

            Debug.Assert(type.IsValueType, SR.Check_TypeMustBeValueType);
        }

        public static T Property<T>(T value) where T : class
        {
            Require.Property(value);

            return value;
        }

        public static string PropertyNotEmpty(string value)
        {
            Require.PropertyNotEmpty(value);

            return value;
        }
    }
}

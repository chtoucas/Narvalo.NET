namespace Narvalo.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    [CLSCompliant(false)]
    public static class __Asserts
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

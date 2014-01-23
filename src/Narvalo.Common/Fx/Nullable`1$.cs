namespace Narvalo.Fx
{
    using System;

    public static partial class NullableExtensions
    {
        //// ValueOr...

        public static T ValueOrDefault<T>(this T? @this)
            where T : struct
        {
            return @this.HasValue ? @this.Value : default(T);
        }

        public static T ValueOrElse<T>(this T? @this, T defaultValue)
            where T : struct
        {
            return @this.HasValue ? @this.Value : defaultValue;
        }

        public static T ValueOrElse<T>(this T? @this, Func<T> defaultValueFactory)
            where T : struct
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this.HasValue ? @this.Value : defaultValueFactory.Invoke();
        }
    }
}

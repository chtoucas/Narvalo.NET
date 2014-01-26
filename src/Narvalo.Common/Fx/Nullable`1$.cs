namespace Narvalo.Fx
{
    using System;

    public static partial class NullableExtensions
    {
        //// ValueOr...

        [Obsolete("Utiliser plutôt l'opérateur ?:")]
        public static T ValueOrDefault<T>(this T? @this)
            where T : struct
        {
            return @this ?? default(T);
        }

        [Obsolete("Utiliser plutôt l'opérateur ?:")]
        public static T ValueOrElse<T>(this T? @this, T defaultValue)
            where T : struct
        {
            return @this ?? defaultValue;
        }

        [Obsolete("Utiliser plutôt l'opérateur ?:")]
        public static T ValueOrElse<T>(this T? @this, Func<T> defaultValueFactory)
            where T : struct
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return @this ?? defaultValueFactory.Invoke();
        }
    }
}

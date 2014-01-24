namespace Narvalo.Fx
{
    /// <summary>
    /// Fournit des méthodes d'extension pour <see cref="Narvalo.Fx.Maybe{T}"/>.
    /// </summary>
    public static class MaybeExtensions
    {
        public static Maybe<TResult> Cast<T, TResult>(this Maybe<T> @this) where T : TResult
        {
            return @this.Map(_ => (TResult)_);
        }

        public static T? ToNullable<T>(this Maybe<T> @this) where T : struct
        {
            // NB: Dans un style plus axiomatique, une manière plus compliquée d'écrire les choses serait :
            // return @this.Match(_ => _, () => (T?)null);
            return @this.IsSome ? (T?)@this.Value : null;
        }

        public static T? ToNullable<T>(this Maybe<T?> @this) where T : struct
        {
            return @this.IsSome ? @this.Value : null;
        }
    }
}

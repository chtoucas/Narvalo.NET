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
            // NB: Une manière plus compliquée, mais dans un style plus constructif, 
            // d'écrire les choses :
            // return @this.Match(_ => _, () => (T?)null);
            return @this.IsSome ? @this.Value : (T?)null;
        }

        public static bool TrySet<T>(this Maybe<T> @this, out T value) where T : struct
        {
            if (@this.IsSome) {
                value = @this.Value;
                return true;
            }
            else {
                value = default(T);
                return false;
            }
        }
    }
}

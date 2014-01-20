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
            return @this.Match(_ => _, () => (T?)null);
        }

        public static bool TrySet<T>(this Maybe<T> @this, out T value) where T : struct
        {
            T? tmp = @this.ToNullable();

            if (tmp.HasValue) {
                value = tmp.Value;
                return true;
            }
            else {
                value = default(T);
                return false;
            }
        }
    }
}

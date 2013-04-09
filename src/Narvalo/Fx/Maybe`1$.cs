namespace Narvalo.Fx
{
    using System;

    public static class MaybeExtensions
    {
        // XXX
        public static Maybe<TResult> Cast<T, TResult>(this Maybe<T> option) where T : TResult
        {
            return option.Map(_ => (TResult)_);
        }

        // XXX
        public static Maybe<T> ThrowIfNone<T>(this Maybe<T> option, Exception ex)
        {
            if (option.IsNone) {
                throw ex;
            }
            return option;
        }

        // XXX
        public static Maybe<T> ThrowIfNone<T>(this Maybe<T> option, Func<Exception> exceptionFactory)
        {
            Requires.NotNull(exceptionFactory, "exceptionFactory");

            if (option.IsNone) {
                throw exceptionFactory();
            }
            return option;
        }

        #region + Match +

        public static TResult Match<TSource, TResult>(
            this Maybe<TSource> option,
            Func<TSource, TResult> fun,
            TResult defaultValue)
        {
            return option.Map(fun).ValueOrElse(defaultValue);
        }

        public static TResult Match<TSource, TResult>(
            this Maybe<TSource> option,
            Func<TSource, TResult> fun,
            Func<TResult> defaultValueFactory)
        {
            return option.Map(fun).ValueOrElse(defaultValueFactory);
        }

        #endregion

        #region + Run +

        // FIXME
        public static void Run<T>(this Maybe<T> option, Action<T> onSome, Action onNone)
        {
            Func<T, Unit> fun = _ => { onSome(_); return Unit.Single; };
            Func<Unit> factory = () => { onNone(); return Unit.Single; };

            option.Match(fun, factory);
        }

        public static void WhenSome<T>(this Maybe<T> option, Action<T> action)
        {
            option.Run(action, () => { });
        }

        public static void WhenNone<T>(this Maybe<T> option, Action action)
        {
            option.Run(_ => { }, action);
        }

        #endregion

        public static T? ToNullable<T>(this Maybe<T> option) where T : struct
        {
            return option.Match(_ => _, () => (T?)null);
        }

        public static bool TrySet<T>(this Maybe<T> option, out T value) where T : struct
        {
            T? tmp = option.ToNullable();

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

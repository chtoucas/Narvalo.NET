namespace Narvalo.Fx
{
    using System;

    public static class MayFunc
    {
        public static Func<Maybe<Unit>> FromAction(Action action)
        {
            return () => { action.Invoke(); return Maybe<Unit>.None; };
        }

        public static Func<T, Maybe<Unit>> FromAction<T>(Action<T> action)
        {
            return _ => { action.Invoke(_); return Maybe<Unit>.None; };
        }
    }
}

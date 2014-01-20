namespace Narvalo.Fx
{
    using System;

    public static class MayFunc
    {
        public static MayFunc<Unit> FromAction(Action action)
        {
            return () => { action.Invoke(); return Maybe<Unit>.None; };
        }

        public static MayFunc<T, Unit> FromAction<T>(Action<T> action)
        {
            return _ => { action.Invoke(_); return Maybe<Unit>.None; };
        }
    }
}

namespace Narvalo.Fx
{
    using System;

    public static class IOAction
    {
        public static Func<Unit> FromAction(Action action)
        {
            return () => { action(); return Unit.Single; };
        }

        public static Func<T, Unit> FromAction<T>(Action<T> action)
        {
            return _ => { action(_); return Unit.Single; };
        }
    }
}

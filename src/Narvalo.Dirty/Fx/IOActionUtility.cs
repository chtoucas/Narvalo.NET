namespace Narvalo.Fx
{
    using System;

    public static class IOActionUtility
    {
        public static IOAction FromAction(Action action)
        {
            return () => { action(); return Unit.Single; };
        }

        public static IOAction<T> FromAction<T>(Action<T> action)
        {
            return _ => { action(_); return Unit.Single; };
        }
    }
}

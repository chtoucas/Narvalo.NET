﻿namespace Narvalo.Fx
{
    using System;

    public static class MayFuncExtensions
    {
        public static Action AsAction(this MayFunc<Unit> kun)
        {
            Requires.Object(kun);

            return () => kun();
        }

        public static Action<T> AsAction<T>(this MayFunc<T, Unit> kun)
        {
            Requires.Object(kun);

            return _ => kun(_);
        }

        public static Func<T, Maybe<X>> AsFunc<T, X>(this MayFunc<T, X> kun)
        {
            Requires.Object(kun);

            return _ => kun(_);
        }

        #region + When & Unless +

        public static MayFunc<Unit> Unless(this MayFunc<Unit> kun, bool predicate)
        {
            Requires.Object(kun);

            return kun.When(!predicate);
        }

        public static MayFunc<T, Unit> Unless<T>(this MayFunc<T, Unit> kun, bool predicate)
        {
            Requires.Object(kun);

            return kun.When(!predicate);
        }

        public static MayFunc<Unit> When(this MayFunc<Unit> kun, bool predicate)
        {
            Requires.Object(kun);

            return predicate ? kun : () => Maybe.Unit;
        }

        public static MayFunc<T, Unit> When<T>(this MayFunc<T, Unit> kun, bool predicate)
        {
            Requires.Object(kun);

            return predicate ? kun : _ => Maybe.Unit;
        }

        #endregion
    }
}

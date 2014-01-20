namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    //public static partial class Maybe
    //{
    //    public static Maybe<T3[]> Zip<T1, T2, T3>(
    //        T1[] list1,
    //        T2[] list2,
    //        Func<T1, T2, Maybe<T3>> fun)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public static Maybe<IEnumerable<T3>> Zip<T1, T2, T3>(
    //        IEnumerable<T1> list1,
    //        IEnumerable<T2> list2,
    //        Func<T1, T2, Maybe<T3>> fun)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Maybe<T> Then(Action<T> onSome, Action onNone)
    //    {
    //        return OnSome(onSome).OnNone(onNone);

    //        Func<T, Unit> fun = _ => { onSome(_); return Unit.Single; };
    //        Func<Unit> factory = () => { onNone(); return Unit.Single; };

    //        Match(fun, factory);

    //        return this;
    //    }
    //}
}

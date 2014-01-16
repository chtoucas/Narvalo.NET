namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    //public abstract partial class Either<TLeft, TRight>
    //{
    //    // TODO: Implémenter IEquatable<LeftImpl> ?
    //    sealed class LeftImpl : Either<TLeft, TRight>
    //    {
    //        public TLeft Value { get { return LeftValue; } }

    //        public override TResult Switch<TResult>(
    //            Func<TLeft, TResult> caseLeft,
    //            Func<TRight, TResult> caseRight)
    //        {
    //            return caseLeft(LeftValue);
    //        }

    //        public override void Switch(
    //            Action<TLeft> caseLeft,
    //            Action<TRight> caseRight)
    //        {
    //            caseLeft(LeftValue);
    //        }
    //    }

    //    // TODO: Implémenter IEquatable<RightImpl> ?
    //    sealed class RightImpl : Either<TLeft, TRight>
    //    {
    //        public TRight Value { get { return RightValue; } }

    //        public override TResult Switch<TResult>(
    //            Func<TLeft, TResult> caseLeft,
    //            Func<TRight, TResult> caseRight)
    //        {
    //            return caseRight(RightValue);
    //        }

    //        public override void Switch(
    //            Action<TLeft> caseLeft,
    //            Action<TRight> caseRight)
    //        {
    //            caseRight(RightValue);
    //        }
    //    }
    //}

    //public abstract class EitherBase<TLeft, TRight>
    //{
    //    public abstract TResult Switch<TResult>(
    //        Func<TLeft, TResult> caseLeft,
    //        Func<TRight, TResult> caseRight);

    //    public abstract void Switch(
    //        Action<TLeft> caseLeft,
    //        Action<TRight> caseRight);
    //}

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
    //}
}

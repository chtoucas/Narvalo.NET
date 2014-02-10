// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Runtime.ExceptionServices;

    public static class Output
    {
        static readonly Output<Unit> Unit_ = Success(Narvalo.Fx.Unit.Single);

        public static Output<Unit> Unit { get { return Unit_; } }

        //// Return

        public static Output<T> Success<T>(T value)
        {
            return Output<T>.η(value);
        }

        public static Output<T> Failure<T>(ExceptionDispatchInfo exceptionInfo)
        {
            return Output<T>.η(exceptionInfo);
        }

        //// Join

        public static Output<T> Join<T>(Output<Output<T>> square)
        {
            return Output<T>.μ(square);
        }
    }
}

// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Runtime.ExceptionServices;

    public static class Output
    {
        public static Output<T> Success<T>(T value)
        {
            return Output<T>.η(value);
        }

        public static Output<T> Failure<T>(ExceptionDispatchInfo exceptionInfo)
        {
            return Output<T>.η(exceptionInfo);
        }
    }
}

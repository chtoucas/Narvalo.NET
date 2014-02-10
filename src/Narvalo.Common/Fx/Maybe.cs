// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    public static class Maybe
    {
        static readonly Maybe<Unit> None_ = Maybe<Unit>.None;
        static readonly Maybe<Unit> Unit_ = Create(Narvalo.Fx.Unit.Single);

        public static Maybe<Unit> None { get { return None_; } }

        public static Maybe<Unit> Unit { get { return Unit_; } }

        //// Create

        public static Maybe<T> Create<T>(T value)
        {
            return Maybe<T>.η(value);
        }

        public static Maybe<T> Create<T>(T? value) where T : struct
        {
            return value.HasValue ? Maybe<T>.η(value.Value) : Maybe<T>.None;
        }

        //// Join

        public static Maybe<T> Join<T>(Maybe<Maybe<T>> square)
        {
            return Maybe<T>.μ(square);
        }
    }
}

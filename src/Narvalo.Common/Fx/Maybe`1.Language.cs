// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;

    public partial class Maybe<T>
    {
        //// Filter

        public Maybe<T> Filter(Func<T, bool> predicate)
        {
            Require.NotNull(predicate, "predicate");

            return Bind(_ => predicate.Invoke(_) ? this : Maybe<T>.None);
        }

        //// Match

        public TResult Match<TResult>(Func<T, TResult> selector, TResult defaultValue)
        {
            return Map(selector).ValueOrElse(defaultValue);
        }

        public TResult Match<TResult>(Func<T, TResult> selector, Func<TResult> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return Match(selector, defaultValueFactory.Invoke());
        }

        //// OnSome

        public Maybe<T> OnSome(Action<T> action)
        {
            Require.NotNull(action, "action");

            if (IsSome) {
                action.Invoke(Value);
            }

            return this;
        }

        //// OnNone

        public Maybe<T> OnNone(Action action)
        {
            Require.NotNull(action, "action");

            if (IsNone) {
                action.Invoke();
            }

            return this;
        }
    }
}

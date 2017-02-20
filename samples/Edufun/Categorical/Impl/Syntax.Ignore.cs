// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Categorical.Impl
{
    using System;

    public partial class Applicative<T>
    {
        // [GHC.Base] (<*) = liftA2 const
        // [Control.Applicative] u <* v = pure const <*> u <*> v
        public Applicative<T> Ignore<TResult>(Applicative<TResult> other)
        {
            //Func<TResult, Applicative<T>> me = _ => this;
            //Func<T, Func<TResult, Applicative<T>>> always1 = _ => me;

            //var applicative = Applicative.Of(always1);
            //Applicative<Func<TResult, Applicative<T>>> second = this.Gather(applicative);
            //Applicative<Applicative<T>> third = other.Gather(second);

            throw new NotImplementedException();
        }
    }
}

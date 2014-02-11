// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Runtime.CompilerServices;

    /*!
     * On Monads
     * =========
     * 
     * You shouldn't be afraid of the monad! You don't need to understand the theory behind to make good use of it, really.
     * In fact, I guess that the monad theory, or more precisely category theory, has influenced the design of many
     * parts of the .NET framework ; Linq and the Reactive Extensions being the most obvious proofs of that.
     * The .NET type system is not rich enough to make very general monadic constructions but it gives developpers
     * access to some powerful (untold) monadic concepts in a very friendly way.
     * 
     * Sample monads
     * -------------
     * 
     * + Nullable<T>, an additive monad
     * + Maybe<T>, an additive monad
     * + Identity<T>, a (co)monad
     * + Output<T>
     * + Either<TLeft, TRight>
     * 
     * Other monads:
     * + Func<T>
     * + Lazy<T>
     * + Task<T>
     * + IEnumerable<T>
     * 
     * Sample signatures
     * -----------------
     * 
     * Monad definition:
     * + Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> kun)
     * + Monad<TResult> Map<TResult>(Func<T, TResult> selector)
     * + static Monad<T> Return(T value)
     * + static Monad<T> Join(Monad<Monad<T>> square)
     * 
     * Comonad definition:
     * + Comonad<TResult> Cobind<TResult>(Func<Comonad<T>, TResult> kun)
     * + Comonad<TResult> Map<TResult>(Func<T, TResult> selector)
     * + static T Extract(Comonad<T> comonad)
     * + static Comonad<Comonad<T>> Duplicate(Comonad<T> comonad)
     * 
     * References
     * ----------
     * 
     * + [Wes Dyer]: http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx
     * + [Lippert]: http://ericlippert.com/category/monads/
     * + [Meijer]: http://laser.inf.ethz.ch/2012/slides/Meijer/
     * + Stephen Toub on the Task Comonad:
     *   http://blogs.msdn.com/b/pfxteam/archive/2013/04/03/tasks-monads-and-linq.aspx
     * + [Haskell]: http://www.haskell.org/onlinereport/monad.html
     * 
     * Implementations in .NET:
     * + [iSynaptic.Commons]: https://github.com/iSynaptic/iSynaptic.Commons
     * + [SharpMaLib]: http://sharpmalib.codeplex.com/
     */

    /// <summary>
    /// The <b>Narvalo.Fx</b> namespace contains classes and interfaces inspired by functional programming.
    /// </summary>
    [CompilerGeneratedAttribute]
    class NamespaceDoc
    {
    }
}

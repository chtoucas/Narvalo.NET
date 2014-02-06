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
     * Sample signature
     * ----------------
     * 
     * Core monadic methods:
     * + Monad<TResult> Bind<TResult>(Func<T, Monad<TResult>> kun)
     * + Monad<TResult> Map<TResult>(Func<T, TResult> selector)
     * + static Monad<T> η(T value)
     * + static Monad<T> μ(Monad<Monad<T>> square)
     * 
     * References
     * ----------
     * 
     * + [Wes Dyer]: http://blogs.msdn.com/b/wesdyer/archive/2008/01/11/the-marvels-of-monads.aspx
     * + [Lippert]: http://ericlippert.com/category/monads/
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

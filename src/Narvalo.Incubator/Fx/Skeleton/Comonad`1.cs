﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

#define COMONAD_VIA_COBIND

namespace Narvalo.Fx.Skeleton
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    sealed class Comonad<T>
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "cokun",
            Justification = "Monad template definition.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Monad template definition.")]
        public Comonad<TResult> Extend<TResult>(Cokunc<T, TResult> cokun)
        {
#if COMONAD_VIA_COBIND
            throw new NotImplementedException();
#else
            return δ(this).Map(_ => cokun.Invoke(_));
#endif
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "fun",
            Justification = "Monad template definition.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic",
            Justification = "Monad template definition.")]
        public Comonad<TResult> Map<TResult>(Func<T, TResult> fun)
        {
#if COMONAD_VIA_COBIND
            return Extend(_ => fun(ε(_)));
#else
            throw new NotImplementedException();
#endif
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "monad",
            Justification = "Monad template definition.")]
        internal static T ε(Comonad<T> monad)
        {
            throw new NotImplementedException();
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "monad",
            Justification = "Monad template definition.")]
        internal static Comonad<Comonad<T>> δ(Comonad<T> monad)
        {
#if COMONAD_VIA_COBIND
            return monad.Extend(_ => _);
#else
            throw new NotImplementedException();
#endif
        }
    }
}

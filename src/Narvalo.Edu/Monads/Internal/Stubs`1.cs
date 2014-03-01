// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Monads.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Fx;

    static class Stubs<T>
    {
        static readonly Kunc<T, Unit> Ignore_ = _ => Monad.Unit;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Kunc<T, Unit> Ignore { get { return Ignore_; } }
    }
}

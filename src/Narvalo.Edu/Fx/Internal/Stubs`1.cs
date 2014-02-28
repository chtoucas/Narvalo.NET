// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System.Diagnostics.CodeAnalysis;

    public static class Stubs<T>
    {
        static readonly Kunc<T, Unit> Ignore_ = _ => Monad.Unit;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Kunc<T, Unit> Ignore { get { return Ignore_; } }
    }
}

// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using Narvalo.Mvp.Internal;

    public sealed class PresenterFactoryProvider : ServiceProvider<IPresenterFactory>
    {
        static readonly PresenterFactoryProvider Instance_ = new PresenterFactoryProvider();

        PresenterFactoryProvider() : base(() => new DefaultPresenterFactory()) { }

        public static PresenterFactoryProvider Current { get { return Instance_; } }
    }
}

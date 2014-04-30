// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    public sealed class CompositeViewFactoryProvider : ServiceProvider<ICompositeViewFactory>
    {
        static readonly CompositeViewFactoryProvider Instance_ = new CompositeViewFactoryProvider();

        CompositeViewFactoryProvider() : base(() => new DefaultCompositeViewFactory()) { }

        public static CompositeViewFactoryProvider Current { get { return Instance_; } }
    }
}

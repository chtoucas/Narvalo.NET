// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public partial interface ICompositeView : IView
    {
        void Add(IView view);
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(ICompositeViewContract))]
    public partial interface ICompositeView { }

    [ContractClassFor(typeof(ICompositeView))]
    internal abstract class ICompositeViewContract : ICompositeView
    {
        bool IView.ThrowIfNoPresenterBound
        {
            get { return default(bool); }
        }

        event EventHandler IView.Load
        {
            add { }
            remove { }
        }

        void ICompositeView.Add(IView view)
        {
            Requires(view != null);
        }
    }
}

#endif
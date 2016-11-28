// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public partial interface IView<TModel> : IView
    {
        TModel Model { get; set; }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IViewContract<>))]
    public partial interface IView<TModel> { }

    [ContractClassFor(typeof(IView<>))]
    internal abstract class IViewContract<TModel> : IView<TModel>
    {
        TModel IView<TModel>.Model
        {
            get { Ensures(Result<TModel>() != null);  return default(TModel); }
            set { Requires(value != null); }
        }

        bool IView.ThrowIfNoPresenterBound => default(Boolean);

        event EventHandler IView.Load
        {
            add { }
            remove { }
        }
    }
}

#endif
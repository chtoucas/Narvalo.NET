// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public partial interface IPresenter<out TView> : IPresenter where TView : IView
    {
        TView View { get; }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp
{
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IPresenterContract<>))]
    public partial interface IPresenter<out TView> { }

    [ContractClassFor(typeof(IPresenter<>))]
    internal abstract class IPresenterContract<TView> : IPresenter<TView> where TView : IView
    {
        IMessageCoordinator IPresenter.Messages => default(IMessageCoordinator);

        TView IPresenter<TView>.View
        {
            get { Ensures(Result<TView>() != null); return default(TView); }
        }
    }
}

#endif

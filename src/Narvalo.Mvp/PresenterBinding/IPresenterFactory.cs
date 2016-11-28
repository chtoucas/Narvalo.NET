// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;

    public partial interface IPresenterFactory
    {
        IPresenter Create(Type presenterType, Type viewType, IView view);

        void Release(IPresenter presenter);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IPresenterFactoryContract))]
    public partial interface IPresenterFactory { }

    [ContractClassFor(typeof(IPresenterFactory))]
    internal abstract class IPresenterFactoryContract : IPresenterFactory
    {
        IPresenter IPresenterFactory.Create(Type presenterType, Type viewType, IView view)
        {
            Requires(presenterType != null);
            Requires(viewType != null);
            Requires(view != null);
            Ensures(Result<IPresenter>() != null);

            return default(IPresenter);
        }

        void IPresenterFactory.Release(IPresenter presenter)
        {
            // REVIEW: Add a precondition on presenter.
        }
    }
}

#endif
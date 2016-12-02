// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;

    public partial interface ICompositeViewFactory
    {
        ICompositeView Create(Type viewType, IEnumerable<IView> views);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(ICompositeViewFactoryContract))]
    public partial interface ICompositeViewFactory { }

    [ContractClassFor(typeof(ICompositeViewFactory))]
    internal abstract class ICompositeViewFactoryContract : ICompositeViewFactory
    {
        ICompositeView ICompositeViewFactory.Create(Type viewType, IEnumerable<IView> views)
        {
            Requires(viewType != null);
            Requires(views != null);
            Ensures(Result<ICompositeView>() != null);

            return default(ICompositeView);
        }
    }
}

#endif

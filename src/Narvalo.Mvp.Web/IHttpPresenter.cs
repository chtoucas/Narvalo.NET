// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.Web;

    using Narvalo.Mvp;

    public partial interface IHttpPresenter : IPresenter
    {
        HttpContextBase HttpContext { get; }

        IAsyncTaskManager AsyncManager { get; }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Web
{
    using System.Diagnostics.Contracts;
    using System.Web;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IHttpPresenterContract))]
    public partial interface IHttpPresenter { }

    [ContractClassFor(typeof(IHttpPresenter))]
    internal abstract class IHttpPresenterContract : IHttpPresenter
    {
        IAsyncTaskManager IHttpPresenter.AsyncManager
        {
            get
            {
                Ensures(Result<IAsyncTaskManager>() != null);
                return default(IAsyncTaskManager);
            }
        }

        HttpContextBase IHttpPresenter.HttpContext
        {
            get
            {
                Ensures(Result<HttpContextBase>() != null);
                return default(HttpContextBase);
            }
        }

        IMessageCoordinator IPresenter.Messages
        {
            get { return default(IMessageCoordinator); }
        }
    }
}

#endif

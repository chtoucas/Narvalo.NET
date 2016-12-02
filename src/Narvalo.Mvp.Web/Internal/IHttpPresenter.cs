// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Internal
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    using Narvalo.Mvp;

    internal partial interface IHttpPresenter : IPresenter
    {
        [SuppressMessage("Microsoft.Design",
           "CA1044:PropertiesShouldNotBeWriteOnly",
           Justification = "The read part is provided by the public interface.")]
        HttpContextBase HttpContext { set; }

        [SuppressMessage("Microsoft.Design",
            "CA1044:PropertiesShouldNotBeWriteOnly",
            Justification = "The read part is provided by the public interface.")]
        IAsyncTaskManager AsyncManager { set; }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.Web.Internal
{
    using System.Web;
    using System.Diagnostics.Contracts;

    using Narvalo.Mvp;

    [ContractClass(typeof(IHttpPresenterContract))]
    internal partial interface IHttpPresenter { }

    [ContractClassFor(typeof(IHttpPresenter))]
    internal abstract class IHttpPresenterContract : IHttpPresenter
    {
        IAsyncTaskManager IHttpPresenter.AsyncManager
        {
            set
            {
                Contract.Requires(value != null);
            }
        }

        HttpContextBase IHttpPresenter.HttpContext
        {
            set
            {
                Contract.Requires(value != null);
            }
        }

        IMessageCoordinator IPresenter.Messages
        {
            get { return default(IMessageCoordinator); }
        }
    }
}

#endif


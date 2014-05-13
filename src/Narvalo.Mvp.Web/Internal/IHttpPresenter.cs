// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Internal
{
    using System.Web;

    internal interface IHttpPresenter : IPresenter
    {
        HttpContextBase HttpContext { set; }

        IAsyncTaskManager AsyncManager { set; }
    }
}

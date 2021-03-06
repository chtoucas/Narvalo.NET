﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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

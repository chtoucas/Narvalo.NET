// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Views
{
    using System;
    using Narvalo.Mvp;

    public interface IRedirectView : IView
    {
        event EventHandler ActionAccepted;
    }
}

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWindowsForms
{
    using System;

    using Narvalo.Mvp;

    public interface IMainView : IView
    {
        event EventHandler TextBoxTextChanged;
    }
}

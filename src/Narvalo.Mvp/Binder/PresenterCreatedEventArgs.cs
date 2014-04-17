﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using System;

    public sealed class PresenterCreatedEventArgs : EventArgs
    {
        readonly IPresenter _presenter;

        public PresenterCreatedEventArgs(IPresenter presenter)
        {
            _presenter = presenter;
        }

        public IPresenter Presenter
        {
            get { return _presenter; }
        }
    }
}

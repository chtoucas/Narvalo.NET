// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System;

    public sealed class PresenterEventArgs : EventArgs
    {
        private readonly IPresenter _presenter;

        public PresenterEventArgs(IPresenter presenter)
        {
            _presenter = presenter;
        }

        public IPresenter Presenter
        {
            get { return _presenter; }
        }
    }
}

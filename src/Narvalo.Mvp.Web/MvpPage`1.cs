// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Properties;

    public abstract class MvpPage<TViewModel> : MvpPage, IView<TViewModel>
    {
        private TViewModel _model;

        protected MvpPage() : base(true) { }

        protected MvpPage(bool throwIfNoPresenterBound) : base(throwIfNoPresenterBound) { }

        public TViewModel Model
        {
            get
            {
                if (_model == null)
                {
                    throw new InvalidOperationException(Strings.MvpUserControl_ModelPropertyIsNull);
                }

                return _model;
            }

            set
            {
                _model = value;
            }
        }
    }
}

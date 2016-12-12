// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;

    using Narvalo.Mvp;
    using Narvalo.Mvp.Web.Properties;

    public class MvpUserControl<TViewModel> : MvpUserControl, IView<TViewModel>
    {
        private TViewModel _model;

        protected MvpUserControl() : base(true) { }

        protected MvpUserControl(bool throwIfNoPresenterBound) : base(throwIfNoPresenterBound) { }

        public TViewModel Model
        {
            get
            {
                Warrant.NotNullUnconstrained<TViewModel>();

                if (_model == null)
                {
                    throw new InvalidOperationException(Strings.MvpUserControl_ModelPropertyIsNull);
                }

                return _model;
            }

            set
            {
                Require.NotNullUnconstrained(value, nameof(value));

                _model = value;
            }
        }
    }
}

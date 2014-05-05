// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;

    public class MvpUserControl<TViewModel> : MvpUserControl, IView<TViewModel>
    {
        TViewModel _model;

        protected MvpUserControl() { }

        public TViewModel Model
        {
            get
            {
                if (_model == null) {
                    throw new InvalidOperationException(
                        "The Model property is currently null, however it should have been automatically initialized by the presenter. This most likely indicates that no presenter was bound to the control. Check your presenter bindings.");
                }

                return _model;
            }
            set { _model = value; }
        }
    }
}

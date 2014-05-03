// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;
    using System.ComponentModel;

    public partial class MvpUserControl<TViewModel> : MvpUserControl, IView<TViewModel>
    {
        TViewModel _model;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TViewModel Model
        {
            get
            {
                if (_model == null) {
                    throw new InvalidOperationException(
                        "The Model property is currently null, however it should have been automatically initialized by the presenter.");
                }

                return _model;
            }

            set { _model = value; }
        }
    }
}

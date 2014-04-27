// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    public abstract class MvpCommand<TModel> : MvpCommand, IView<TModel>
    {
        TModel _model;

        public TModel Model
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

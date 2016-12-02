// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System;

    using Narvalo.Mvp.Properties;

    public class MvpCommand<TViewModel> : MvpCommand, IView<TViewModel>
    {
        private TViewModel _model;

        public TViewModel Model
        {
            get
            {
                if (_model == null)
                {
                    throw new InvalidOperationException(Strings.MvpCommand_ModelIsNull);
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

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System;

    using Narvalo.Mvp.Properties;

    using static System.Diagnostics.Contracts.Contract;

    public class MvpCommand<TViewModel> : MvpCommand, IView<TViewModel>
    {
        private TViewModel _model;

        public MvpCommand() : base() { }

        public MvpCommand(bool throwIfNoPresenterBound) : base(throwIfNoPresenterBound) { }

        public TViewModel Model
        {
            get
            {
                Ensures(Result<TViewModel>() != null);

                if (_model == null)
                {
                    throw new InvalidOperationException(Strings.MvpCommand_ModelPropertyIsNull);
                }

                return _model;
            }

            set
            {
                Require.Property(value);

                _model = value;
            }
        }
    }
}

// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Mvp.CommandLine.Internal;
    using Narvalo.Mvp.PresenterBinding;

    using static System.Diagnostics.Contracts.Contract;

    public class MvpCommand : IView, ICommand
    {
        private readonly bool _throwIfNoPresenterBound;

        private bool _initialized = false;
        private PresenterBinder _presenterBinder;

        protected MvpCommand() : this(true) { }

        protected MvpCommand(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
        }

        public event EventHandler Completed;
        public event EventHandler Load;

        public bool ThrowIfNoPresenterBound => _throwIfNoPresenterBound;

        public void Init() => Init(true);

        public void Run()
        {
            Init();

            OnLoad(EventArgs.Empty);

            _presenterBinder.Release();

            OnCompleted(EventArgs.Empty);
        }

        protected virtual void Init(bool initializing)
        {
            if (!_initialized)
            {
                if (initializing)
                {
                    _presenterBinder = PresenterBinderFactory.Create(this);
                    _presenterBinder.PerformBinding();
                }

                _initialized = true;
            }
        }

        protected virtual void OnCompleted(EventArgs e) => Completed?.Invoke(this, e);

        protected virtual void OnLoad(EventArgs e) => Load?.Invoke(this, e);
    }
}

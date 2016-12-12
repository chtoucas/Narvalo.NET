// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

    using Narvalo.Mvp.CommandLine.Internal;
    using Narvalo.Mvp.PresenterBinding;

    public class MvpCommand : IView, ICommand
    {
        private readonly bool _throwIfNoPresenterBound;
        private readonly PresenterBinder _presenterBinder;

        protected MvpCommand() : this(true) { }

        protected MvpCommand(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;
            _presenterBinder = PresenterBinderFactory.Create(this);
        }

        public event EventHandler Completed;
        public event EventHandler Load;

        public bool ThrowIfNoPresenterBound => _throwIfNoPresenterBound;

        public void Run()
        {
            _presenterBinder.PerformBinding();

            OnLoad(EventArgs.Empty);

            _presenterBinder.Release();

            OnCompleted(EventArgs.Empty);
        }

        protected virtual void OnCompleted(EventArgs e) => Completed?.Invoke(this, e);

        protected virtual void OnLoad(EventArgs e) => Load?.Invoke(this, e);

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_presenterBinder != null);
        }

#endif
    }
}

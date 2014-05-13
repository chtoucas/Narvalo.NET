// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using System;
    using Narvalo.Mvp.PresenterBinding;

    public abstract class MvpCommand : IView, ICommand, IDisposable
    {
        readonly bool _throwIfNoPresenterBound;

        bool _disposed = false;
        PresenterBinder _presenterBinder;

        protected MvpCommand() : this(true) { }

        protected MvpCommand(bool throwIfNoPresenterBound)
        {
            _throwIfNoPresenterBound = throwIfNoPresenterBound;

            _presenterBinder = PresenterBinderFactory.Create(this);
            _presenterBinder.PerformBinding();
        }

        public event EventHandler Completed;
        public event EventHandler Load;

        public bool ThrowIfNoPresenterBound
        {
            get { return _throwIfNoPresenterBound; }
        }

        public void Execute()
        {
            OnLoad();
            ExecuteCore();
            OnCompleted();
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void ExecuteCore() { }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (_presenterBinder != null) {
                        _presenterBinder.Release();
                        _presenterBinder = null;
                    }
                }

                _disposed = true;
            }
        }

        protected virtual void OnLoad()
        {
            var localHandler = Load;

            if (localHandler != null) {
                localHandler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnCompleted()
        {
            var localHandler = Completed;

            if (localHandler != null) {
                localHandler(this, EventArgs.Empty);
            }
        }
    }
}

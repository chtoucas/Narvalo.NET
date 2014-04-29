// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using Narvalo.Mvp.Binder;

    public abstract class MvpCommand : IView, ICommand, IDisposable
    {
        bool _disposed = false;
        PresenterBinder _presenterBinder;

        protected MvpCommand()
        {
            _presenterBinder = new PresenterBinder(this);
            _presenterBinder.PerformBinding();
        }

        public event EventHandler Completed;
        public event EventHandler Load;

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

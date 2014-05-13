// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    // TODO: Scheduler
    public sealed class ReactiveMessageBus : IMessageBus, IDisposable
    {
        Subject<object> _subject;

        bool _disposed = false;

        public ReactiveMessageBus()
        {
            _subject = new Subject<object>();
        }

        public bool Closed
        {
            get { return !_disposed; }
        }

        public void Close()
        {
            Dispose_(true);
        }

        public void Publish<T>(T message)
        {
            ThrowIfDisposed_();

            _subject.OnNext(message);
        }

        public IObservable<T> Register<T>()
        {
            ThrowIfDisposed_();

            return _subject.OfType<T>();
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            ThrowIfDisposed_();

            Require.NotNull(onNext, "onNext");

            _subject.OfType<T>().Subscribe(onNext);
        }

        public void Dispose()
        {
            Dispose_(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        void Dispose_(bool disposing)
        {
            if (!_disposed) {
                if (disposing) {
                    if (_subject != null) {
                        _subject.Dispose();
                        _subject = null;
                    }
                }

                _disposed = true;
            }
        }

        void ThrowIfDisposed_()
        {
            if (_disposed) {
                throw new ObjectDisposedException(
                    typeof(ReactiveMessageBus).FullName,
                    "Messages can't be published or subscribed to after the message bus has been disposed.");
            }
        }
    }
}

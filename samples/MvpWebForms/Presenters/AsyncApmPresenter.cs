// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Presenters
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using MvpWebForms.Views;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apm")]
    public sealed class AsyncApmPresenter : HttpPresenterOf<AsyncModel>
    {
        static readonly Action Thunk_ = () => Thread.Sleep(10);

        public AsyncApmPresenter(IView<AsyncModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.RecordViewLoad();

            AsyncManager.RegisterAsyncTask(BeginInvoke, EndInvoke, null);
        }

        IAsyncResult BeginInvoke(object sender, EventArgs e, AsyncCallback cb, object state)
        {
            View.Model.RecordAsyncStarted();

            return Thunk_.BeginInvoke(cb, state);
        }

        void EndInvoke(IAsyncResult ar)
        {
            Thunk_.EndInvoke(ar);

            View.Model.RecordAsyncEnded();
        }
    }
}

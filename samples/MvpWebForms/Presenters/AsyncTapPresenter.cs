// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace MvpWebForms.Presenters
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MvpWebForms.Views;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;

    public sealed class AsyncTapPresenter : HttpPresenterOf<AsyncModel>
    {
        private static readonly Action s_Thunk = () => Thread.Sleep(10);

        public AsyncTapPresenter(IView<AsyncModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        private void Load(object sender, EventArgs e)
        {
            View.Model.RecordViewLoad();

            AsyncManager.RegisterAsyncTask(InvokeAsync);
        }

        private async Task InvokeAsync()
        {
            View.Model.RecordAsyncStarted();

            await Task.Run(s_Thunk);

            View.Model.RecordAsyncEnded();
        }
    }
}

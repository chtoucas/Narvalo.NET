namespace MvpWebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using MvpWebForms.Views;

    public sealed class SharedPresenter : PresenterOf<StringModel>
    {
        public SharedPresenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += (sender, e) =>
                View.Model.Message = String.Format(@"Presenter instance: {0}", Guid.NewGuid());
        }
    }
}
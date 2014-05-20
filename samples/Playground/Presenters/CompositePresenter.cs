namespace Playground.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.Views;

    public sealed class CompositePresenter : PresenterOf<StringModel>
    {
        public CompositePresenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += (sender, e) =>
                View.Model.Message = String.Format(@"Presenter instance: {0}", Guid.NewGuid());
        }
    }
}

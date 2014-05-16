namespace Playground.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.Views;

    public class CompositeDemoPresenter : PresenterOf<StringModel>
    {
        public CompositeDemoPresenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Message = String.Format(
                "This message was set by the presenter. Here's a new guid to demonstrate that all views are sharing the one presenter instance: {0}",
                Guid.NewGuid());
        }
    }
}

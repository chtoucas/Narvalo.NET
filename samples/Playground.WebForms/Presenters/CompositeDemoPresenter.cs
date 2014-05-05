namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Views;

    public class CompositeDemoPresenter
        : Presenter<ICompositeDemoView>
    {
        public CompositeDemoPresenter(ICompositeDemoView view)
            : base(view)
        {
            View.Load += new EventHandler(View_Load);
        }

        void View_Load(object sender, EventArgs e)
        {
            View.Model.Message = string.Format(
                "This message was set by the presenter. Here's a new guid to demonstrate that all views are sharing the one presenter instance: {0}",
                Guid.NewGuid());
        }
    }
}

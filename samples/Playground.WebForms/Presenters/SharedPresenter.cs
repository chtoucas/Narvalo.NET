namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Views.Models;

    public class SharedPresenter : PresenterOf<SharedPresenterViewModel>
    {
        public SharedPresenter(IView<SharedPresenterViewModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Message = String.Format(@"Presenter instance: {0}", Guid.NewGuid());
        }
    }
}
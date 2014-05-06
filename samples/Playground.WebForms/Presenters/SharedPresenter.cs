namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Views.Models;

    public class SharedPresenter
        : Presenter<IView<SharedPresenterViewModel>>
    {
        public SharedPresenter(IView<SharedPresenterViewModel> view)
            : base(view)
        {
            View.Model = new SharedPresenterViewModel();

            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Message = string.Format(@"Presenter instance: {0}", Guid.NewGuid());
        }
    }
}
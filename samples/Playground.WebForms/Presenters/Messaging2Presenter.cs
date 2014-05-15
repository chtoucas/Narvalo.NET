namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Services;
    using Playground.WebForms.Views;

    public class Messaging2Presenter : PresenterOf<StringModel>
    {
        public Messaging2Presenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            Messages.Subscribe<Widget>(_ =>
            {
                View.Model.Message += String.Format("Presenter 2 received widget {0}.", _.Id);
            });

            Messages.Subscribe<Guid>(_ =>
            {
                View.Model.Message += " Presenter 2 received an unexpected GUID message! Oops.";
            });
        }
    }
}

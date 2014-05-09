namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Domain;
    using Playground.WebForms.Views.Models;

    public class Messaging2Presenter : HttpPresenterOf<MessagingModel>
    {
        public Messaging2Presenter(IView<MessagingModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            Messages.Subscribe<Widget>(_ =>
            {
                View.Model.DisplayText += String.Format("Presenter 2 received widget {0}.", _.Id);
            });

            Messages.Subscribe<Guid>(_ =>
            {
                View.Model.DisplayText += " Presenter 2 received an unexpected GUID message! Oops.";
            });
        }
    }
}

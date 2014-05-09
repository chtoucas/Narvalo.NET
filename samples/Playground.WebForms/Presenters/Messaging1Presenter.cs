namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Domain;
    using Playground.WebForms.Views.Models;

    public class Messaging1Presenter : HttpPresenterOf<MessagingModel>
    {
        public Messaging1Presenter(IView<MessagingModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            var widget = new Widget {
                Id = 123,
                Name = "Awesome widget!"
            };

            View.Model.DisplayText = String.Format("Presenter 1 publishes widget {0}.", widget.Id);

            Messages.Publish(widget);
        }
    }
}

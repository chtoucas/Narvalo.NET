namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Domain;
    using Playground.WebForms.Views.Models;

    public class Messaging1Presenter : Presenter<IView<MessagingModel>>
    {
        public Messaging1Presenter(IView<MessagingModel> view)
            : base(view)
        {
            View.Model = new MessagingModel();

            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            var widget = new Widget {
                Id = 123,
                Name = "Awesome widget!"
            };

            View.Model.DisplayText = String.Format("Presenter A published widget {0}.", widget.Id);

            // This publishes the widget to the bus.
            Messages.Publish(widget);
        }
    }
}

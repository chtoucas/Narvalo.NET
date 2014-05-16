namespace Playground.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.Services;
    using Playground.Views;

    public class Messaging1Presenter : PresenterOf<StringModel>
    {
        public Messaging1Presenter(IView<StringModel> view)
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

            View.Model.Message = String.Format("Presenter 1 publishes widget {0}.", widget.Id);

            Messages.Publish(widget);
            Messages.Publish(123456);
        }
    }
}

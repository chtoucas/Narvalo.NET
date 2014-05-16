namespace Playground.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.Views;

    public sealed class Messaging1Presenter : PresenterOf<StringModel>
    {
        public Messaging1Presenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            var message = new StringMessage {
                Content = "Awesome widget!"
            };

            View.Model.Message = String.Format("Presenter 1 publishes message: {0}", message.Content);

            Messages.Publish(message);
            Messages.Publish(123456);
        }
    }
}

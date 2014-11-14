namespace MvpWebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using MvpWebForms.Views;

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
                Content = "Hello from Messaging1Presenter!"
            };

            View.Model.Message = String.Format("Presenter 1 publishes: {0}", message.Content);

            Messages.Publish(message);
            Messages.Publish(123456);
        }
    }
}

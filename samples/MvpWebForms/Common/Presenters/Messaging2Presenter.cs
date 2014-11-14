namespace MvpWebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using MvpWebForms.Views;

    public sealed class Messaging2Presenter : PresenterOf<StringModel>
    {
        public Messaging2Presenter(IView<StringModel> view)
            : base(view)
        {
            View.Load += Load;
        }

        void Load(object sender, EventArgs e)
        {
            View.Model.Message = String.Empty;

            Messages.Subscribe<StringMessage>(_ =>
            {
                View.Model.Message += String.Format("Presenter 2 received: {0}", _.Content);
            });

            Messages.Subscribe<Guid>(_ =>
            {
                View.Model.Message += " Oops. Presenter 2 received an unexpected GUID message! ";
            });
        }
    }
}

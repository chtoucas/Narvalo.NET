namespace MvpWebForms.Presenters
{
    using System;
    using System.Globalization;
    using MvpWebForms.Views;
    using Narvalo.Mvp;

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
                View.Model.Message += String.Format(CultureInfo.InvariantCulture, "Presenter 2 received: {0}", _.Content);
            });

            Messages.Subscribe<Guid>(_ =>
            {
                View.Model.Message += " Oops. Presenter 2 received an unexpected GUID message! ";
            });
        }
    }
}

namespace Playground.WebForms.Presenters
{
    using System;
    using Narvalo.Mvp;
    using Playground.WebForms.Views;

    public class TimeServicePresenter : Presenter<ITimeServiceView>
    {
        public TimeServicePresenter(ITimeServiceView view)
            : base(view)
        {
            View.GetCurrentTimeCalled += (sender, e) => e.Result = e.LocalTime ? DateTime.Now : DateTime.UtcNow;
        }
    }
}
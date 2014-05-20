namespace Playground.Views
{
    using System;
    using Narvalo.Mvp;

    public interface IWidgetsReadOnlyView : IView<WidgetsReadOnlyModel>
    {
        event EventHandler<WidgetIdEventArgs> Finding;

        event EventHandler<WidgetIdEventArgs> FindingApm;

        event EventHandler<WidgetIdEventArgs> FindingTap;
    }
}
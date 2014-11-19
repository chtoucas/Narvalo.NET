namespace MvpWebForms.Views
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Mvp;

    public interface IWidgetsReadOnlyView : IView<WidgetsReadOnlyModel>
    {
        event EventHandler<WidgetIdEventArgs> Finding;

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apm")]
        event EventHandler<WidgetIdEventArgs> FindingApm;

        event EventHandler<WidgetIdEventArgs> FindingTap;
    }
}
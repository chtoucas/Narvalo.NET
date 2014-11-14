namespace MvpWebForms.Presenters
{
    using Narvalo.Mvp;
    using MvpWebForms.Data;
    using MvpWebForms.Views;

    public sealed class WidgetsReadOnlyPresenter
        : Presenter<IWidgetsReadOnlyView, WidgetsReadOnlyModel>
    {
        public WidgetsReadOnlyPresenter(IWidgetsReadOnlyView view)
            : base(view)
        {
            View.Finding += Finding;
        }

        void Finding(object sender, WidgetIdEventArgs e)
        {
            using (var context = new MvpWebFormsContext()) {
                var widget = context.Widgets.Find(e.Id);

                if (widget != null) {
                    View.Model.Widgets.Add(widget);
                    View.Model.ShowResult = true;
                }
            }
        }
    }
}
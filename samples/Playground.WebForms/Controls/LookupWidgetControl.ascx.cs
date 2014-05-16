namespace Playground.Controls
{
    using System;
    using Narvalo.Mvp.Web;
    using Playground.Views;

    public partial class LookupWidgetControl : MvpUserControl<LookupWidgetModel>, ILookupWidgetView
    {
        public event EventHandler<WidgetIdEventArgs> Finding;

        protected void Find_Click(object sender, EventArgs e)
        {
            var id = String.IsNullOrEmpty(WidgetId.Text)
                ? (int?)null
                : Convert.ToInt32(WidgetId.Text);

            if (id.HasValue)
                OnFinding(id.Value);
        }

        void OnFinding(int id)
        {
            if (Finding != null) {
                Finding(this, new WidgetIdEventArgs(id));
            }
        }
    }
}
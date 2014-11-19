namespace MvpWebForms.Controls
{
    using MvpWebForms.Views;
    using Narvalo.Mvp.Web;

    public partial class DynamicallyLoadedControl : MvpUserControl, IDynamicallyLoadedView
    {
        public DynamicallyLoadedControl()
        {
            PreRender += (sender, e) => { if (PresenterWasBound) { cph1.Visible = true; cph2.Visible = false; } };
        }

        public bool PresenterWasBound { get; set; }
    }
}
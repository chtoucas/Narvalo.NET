namespace MvpWebForms.Controls
{
    using Narvalo.Mvp.Web;
    using MvpWebForms.Views;

    public partial class DynamicallyLoadedControl : MvpUserControl, IDynamicallyLoadedView
    {
        public DynamicallyLoadedControl()
        {
            PreRender += (sender, e) => { if (PresenterWasBound) { cph1.Visible = true; cph2.Visible = false; } };
        }

        public bool PresenterWasBound { get; set; }
    }
}
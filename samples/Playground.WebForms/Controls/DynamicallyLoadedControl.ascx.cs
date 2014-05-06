namespace Playground.WebForms.Controls
{
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views;

    public partial class DynamicallyLoadedControl : MvpUserControl, IDynamicallyLoadedView
    {
        public DynamicallyLoadedControl()
        {
            PreRender += (sender, e) => { if (PresenterWasBound) { cph1.Visible = true; cph2.Visible = false; } };
        }

        // We are purposely adding a property to the view as the View Model is usually 
        // initiated by the base presenter class and this control must keep working even
        // if that doesn't happen. Controls that fail to load dynamically fail silently
        // so we need this to be explicit.
        public bool PresenterWasBound { get; set; }
    }
}
namespace Playground.WebForms.Controls
{
    using Narvalo.Mvp.Web;
    using Playground.WebForms.Views;
    using Playground.WebForms.Views.Models;

    public partial class CompositeControl :
        MvpUserControl<CompositeDemoViewModel>, ICompositeDemoView
    {   
    }
}
namespace Playground.WebForms
{
    using System;
    using System.Web.UI;

    public partial class ViewsAddedInPageInit : Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            dynamicallyLoadedControlsPlaceholder.Controls.Add(LoadControl("~/Controls/DynamicallyLoadedControl.ascx"));
        }
    }
}
namespace Playground
{
    using System;
    using System.Web.UI;

    public partial class ViewsAddedInPageInitPage : Page
    {
        public ViewsAddedInPageInitPage()
        {
            Init += Page_Init;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            DynamicPanel.Controls.Add(LoadControl("~/Controls/DynamicallyLoadedControl.ascx"));
        }
    }
}
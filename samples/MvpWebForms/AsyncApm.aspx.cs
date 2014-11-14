namespace MvpWebForms
{
    using System;
    using System.Web.UI;

    public partial class AsyncApmPage : Page
    {
        public AsyncApmPage()
            : base()
        {
            PreRenderComplete += Page_PreRenderComplete;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            AsyncApmControl.Model.RecordPagePreRenderComplete();
        }
    }
}
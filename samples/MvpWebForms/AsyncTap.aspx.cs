namespace MvpWebForms
{
    using System;
    using System.Web.UI;

    public partial class AsyncTapPage : Page
    {
        public AsyncTapPage()
            : base()
        {
            PreRenderComplete += Page_PreRenderComplete;
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            AsyncTapControl.Model.RecordPagePreRenderComplete();
        }
    }
}
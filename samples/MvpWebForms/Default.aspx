<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="MvpWebForms.DefaultPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Platform Agnostic Presenters</h2>
 <ul>
  <li>
   <asp:HyperLink NavigateUrl="~/HelloWorld.aspx" runat=server>Hello world</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/SharedPresenter.aspx" runat=server>Shared presenter</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/Messaging.aspx" runat=server>Cross-presenter communication</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/ViewsAddedInPageInit.aspx" runat=server>Views dynamically loaded in Page_Init</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/Widgets.aspx" runat=server>Data access: CRUD</asp:HyperLink></li>
 </ul>
 <h2>Mix of ASP.NET and Platform Independent Presenters</h2>
 <ul>
  <li>
   <asp:HyperLink NavigateUrl="~/LookupWidget.aspx" runat=server>Data access: Lookup</asp:HyperLink></li>
 </ul>
 <h2>ASP.NET Only Presenters</h2>
 <ul>
  <li>
   <asp:HyperLink NavigateUrl="~/Redirect.aspx" runat=server>Redirect from Presenter on user request</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/Parallel.aspx" runat=server>Parallel Tasks</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/AsyncTap.aspx" runat=server>Async TAP Version</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/AsyncApm.aspx" runat=server>Async APM Version</asp:HyperLink>
   (legacy)</li>
 </ul>
 <h2>Page as View</h2>
 <p>All previous samples use UserControl's as Views, but it also works with Page's.</p>
 <ul>
  <li>
   <asp:HyperLink NavigateUrl="~/PageView.aspx" runat=server>Hello world</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/CompositeView.aspx" runat=server>Shared presenter</asp:HyperLink></li>
 </ul>
</asp:Content>

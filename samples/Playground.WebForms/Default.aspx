<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="Playground.DefaultPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Platform Agnostic Presenters</h2>
 <ul>
  <li>
   <asp:HyperLink NavigateUrl="~/HelloWorld.aspx" runat=server>Hello word</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/SharedPresenter.aspx" runat=server>Shared presenter</asp:HyperLink>
   binding attribute found on the user control</li>
  <li>
   <asp:HyperLink NavigateUrl="~/CompositeView.aspx" runat=server>Shared presenter</asp:HyperLink>
   binding attribute found on the page</li>
  <li>
   <asp:HyperLink NavigateUrl="~/PageView.aspx" runat=server>Page as view</asp:HyperLink></li>
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
   <asp:HyperLink NavigateUrl="~/AsyncTap.aspx" runat=server>Async Task-based Asynchronous Pattern (TAP)</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/AsyncApm.aspx" runat=server>Async Asynchronous Programming Model (APM)</asp:HyperLink></li>
 </ul>
</asp:Content>

<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="Playground.DefaultPage" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <ul>
  <li>
   <asp:HyperLink NavigateUrl="~/HelloWorld.aspx" runat=server>Hello Word</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/SharedPresenter.aspx" runat=server>Shared Presenter</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/CompositeView.aspx" runat=server>Composite View</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/PageView.aspx" runat=server>Page as View</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/Messaging.aspx" runat=server>Cross-Presenter Communication</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/Redirect.aspx" runat=server>Redirect from Presenter on user request</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/ViewsAddedInPageInit.aspx" runat=server>Views Dynamically Loaded in Page_Init</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/LookupWidget.aspx" runat=server>Data Access: Lookup</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/Widgets.aspx" runat=server>Data Access: CRUD</asp:HyperLink></li>    
  <li>
   <asp:HyperLink NavigateUrl="~/Parallel.aspx" runat=server>Parallel Tasks</asp:HyperLink></li>     
  <li>
   <asp:HyperLink NavigateUrl="~/AsyncTap.aspx" runat=server>Async Task-based Asynchronous Pattern (TAP)</asp:HyperLink></li>
  <li>
   <asp:HyperLink NavigateUrl="~/AsyncApm.aspx" runat=server>Async Asynchronous Programming Model (APM)</asp:HyperLink></li> 
 </ul>
</asp:content>

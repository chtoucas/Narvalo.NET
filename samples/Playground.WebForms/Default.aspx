<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="Playground.WebForms.DefaultPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <ul>      
  <li>
   <asp:HyperLink NavigateUrl="~/HelloWorld.aspx" runat=server>Hello Word</asp:HyperLink>
  </li> 
  <li>
   <asp:HyperLink NavigateUrl="~/SharedPresenter.aspx" runat=server>Shared Presenter</asp:HyperLink>
  </li>    
  <li>
   <asp:HyperLink NavigateUrl="~/CompositeView.aspx" runat=server>Composite View</asp:HyperLink>
  </li>
  <li>
   <asp:HyperLink NavigateUrl="~/PageView.aspx" runat=server>Page as View</asp:HyperLink>
  </li>    
  <li>
   <asp:HyperLink NavigateUrl="~/Messaging.aspx" runat=server>Cross-Presenter Communication</asp:HyperLink>
  </li>     
  <li>
   <asp:HyperLink NavigateUrl="~/Redirect.aspx" runat=server>Redirect from Presenter on user request</asp:HyperLink>
  </li>
  <li>
   <asp:HyperLink NavigateUrl="~/ViewsAddedInPageInit.aspx" runat=server>Views Dynamically Loaded in Page_Init</asp:HyperLink>
  </li>
  <li>
   <asp:HyperLink NavigateUrl="~/AsyncApm.aspx" runat=server>Async APM</asp:HyperLink>
  </li>
  <li>
   <asp:HyperLink NavigateUrl="~/AsyncTpl.aspx" runat=server>Async TPL</asp:HyperLink>
  </li>
  <li>
   <asp:HyperLink NavigateUrl="~/Handlers/TimeHandler.ashx" runat=server>IHttpHandler</asp:HyperLink>
  </li>  
  <%--<li><asp:HyperLink NavigateUrl="~/WidgetEdit.aspx" runat=server>Async APM Demo</asp:HyperLink></li>
  <li><asp:HyperLink NavigateUrl="~/WidgetLookup.aspx" runat=server>Async APM Demo</asp:HyperLink></li>--%>
 </ul>
</asp:Content>

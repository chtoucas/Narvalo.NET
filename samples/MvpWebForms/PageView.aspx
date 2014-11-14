<%@ Page Language="C#" CodeBehind="PageView.aspx.cs" Inherits="MvpWebForms.PageViewPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Page as View</h2>
 <p>In this demo, the page itself is acting as the view.</p>
 <p>We prefer to use controls but this approach works too.</p>
 <p class="hello-world">
  <%# Model.Message %>
 </p>
</asp:Content>

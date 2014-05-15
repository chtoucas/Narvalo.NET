<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="Playground.WebForms.DefaultPage" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <h1>ASP.NET Web Forms Model View Presenter</h1>
 <form runat="server">
  <asp:TreeView runat="server" DataSourceID="sitemap" />
  <asp:SiteMapDataSource ID="sitemap" runat="server" ShowStartingNode="false" />
 </form>
</asp:content>

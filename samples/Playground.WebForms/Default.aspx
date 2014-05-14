<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="Playground.WebForms.DefaultPage" %>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="server">
 <h1>ASP.NET Web Forms Model View Presenter</h1>
 <asp:TreeView runat="server" DataSourceID="sitemap" />
 <asp:SiteMapDataSource ID="sitemap" runat="server" ShowStartingNode="false" />
</asp:Content>

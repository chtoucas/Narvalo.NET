<%@ Page Language="C#" CodeBehind="Widgets.aspx.cs" Inherits="MvpWebForms.WidgetsPage" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <h2>Two-Way Data-Binding: Widget CRUD</h2>
 <form runat="server">
  <uc:WidgetsReadWrite runat="server" />
 </form>
</asp:content>

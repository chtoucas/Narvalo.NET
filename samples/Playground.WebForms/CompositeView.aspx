<%@ Page Language="C#" CodeBehind="CompositeView.aspx.cs" Inherits="Playground.CompositeViewPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Composite View</h2>
 <uc:Composite runat="server" />
 <uc:Composite runat="server" />  
 <p><%= Model.Message %></p>
</asp:Content>

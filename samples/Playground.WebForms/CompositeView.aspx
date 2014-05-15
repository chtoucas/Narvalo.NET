<%@ Page Language="C#" CodeBehind="CompositeView.aspx.cs" Inherits="Playground.WebForms.CompositeViewPage" %>

<%@ Register Src="~/Controls/CompositeControl.ascx" TagPrefix="uc" TagName="Composite" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Composite View</h2>
 <uc:Composite runat="server" />
 <uc:Composite runat="server" />  
 <p><%= Model.Message %></p>
</asp:Content>

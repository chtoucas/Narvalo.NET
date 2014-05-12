<%@ Page Language="C#" MasterPageFile="~/Layouts/Site.Master" CodeBehind="CompositeView.aspx.cs"
 Inherits="Playground.WebForms.CompositeViewPage" %>

<%@ Register Src="~/Controls/CompositeControl.ascx" TagPrefix="uc" TagName="Composite" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>Composite View Demo</h1>
 <uc:Composite runat="server" />
 <uc:Composite runat="server" />  
 <p><%= Model.Message %></p>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/Layouts/Site.Master" CodeBehind="WidgetEdit.aspx.cs"
 Inherits="Playground.WebForms.WidgetEditPage" %>

<%@ Register Src="~/Controls/EditWidgetControl.ascx" TagPrefix="uc" TagName="EditWidget" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>2-Way Data-binding Demo: Widget CRUD</h1>
 <uc:EditWidget runat="server" />
</asp:Content>

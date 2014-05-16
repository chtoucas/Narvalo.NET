<%@ Page Language="C#" CodeBehind="WidgetEdit.aspx.cs" Inherits="Playground.WidgetEditPage" %>

<%@ Register Src="~/Controls/EditWidgetControl.ascx" TagPrefix="uc" TagName="EditWidget" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Two-Way Data-Binding: Widget CRUD</h2>
 <uc:EditWidget runat="server" />
</asp:Content>

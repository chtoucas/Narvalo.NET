<%@ Page Async="true" Language="C#" MasterPageFile="~/Layouts/Site.Master"
 CodeBehind="AsyncMessages.aspx.cs" Inherits="Playground.WebForms.AsyncMessages" %>

<%@ Register Src="~/Controls/AsyncMessagesControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>Async Tasks Demo</h1>
 <uc:AsyncMessages runat="server" />
</asp:Content>

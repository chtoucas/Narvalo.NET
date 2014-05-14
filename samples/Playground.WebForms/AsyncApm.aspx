<%@ Page Async="true" Language="C#" CodeBehind="AsyncApm.aspx.cs" Inherits="Playground.WebForms.AsyncApmPage" %>

<%@ Register Src="~/Controls/AsyncApmControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>Async APM Demo</h1>
 <uc:AsyncMessages runat="server" />
</asp:Content>

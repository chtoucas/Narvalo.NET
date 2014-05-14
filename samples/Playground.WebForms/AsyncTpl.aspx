<%@ Page Async="true" Language="C#" CodeBehind="AsyncTpl.aspx.cs" Inherits="Playground.WebForms.AsyncTplPage" %>

<%@ Register Src="~/Controls/AsyncTplControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>Async TPL Demo</h1>
 <uc:AsyncMessages runat="server" />
</asp:Content>

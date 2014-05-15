<%@ Page Async="true" Language="C#" CodeBehind="AsyncTpl.aspx.cs" Inherits="Playground.WebForms.AsyncTplPage" %>

<%@ Register Src="~/Controls/AsyncTplControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h1>Async TPL Demo</h1>
 <p>Each task take 100ms to complete, but the overall time spent is under 200ms.</p>
 <h2>Chronology</h2>
 <uc:AsyncMessages runat="server" />
</asp:Content>

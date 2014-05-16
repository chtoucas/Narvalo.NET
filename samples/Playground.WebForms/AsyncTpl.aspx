<%@ Page Async="true" Language="C#" CodeBehind="AsyncTpl.aspx.cs" Inherits="Playground.AsyncTplPage" %>

<%@ Register Src="~/Controls/AsyncTplControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Async TPL</h2>
 <p>Each task take 100ms to complete, but the overall time spent is under 200ms.</p>
 <h3>Chronology</h3>
 <uc:AsyncMessages runat="server" />
</asp:Content>

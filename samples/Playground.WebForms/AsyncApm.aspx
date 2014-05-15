<%@ Page Async="true" Language="C#" CodeBehind="AsyncApm.aspx.cs" Inherits="Playground.WebForms.AsyncApmPage" %>

<%@ Register Src="~/Controls/AsyncApmControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>
<asp:content contentplaceholderid="content" runat="server">
 <h1>Async APM Demo</h1>
 <p>Each task take 100ms to complete, but the overall time spent is under 200ms.</p>
 <h2>Chronology</h2>
 <uc:AsyncMessages runat="server" />
</asp:content>

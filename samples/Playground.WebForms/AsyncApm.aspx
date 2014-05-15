<%@ Page Async="true" Language="C#" CodeBehind="AsyncApm.aspx.cs" Inherits="Playground.WebForms.AsyncApmPage" %>

<%@ Register Src="~/Controls/AsyncApmControl.ascx" TagPrefix="uc" TagName="AsyncMessages" %>

<asp:content ContentPlaceHolderID="MainContent" runat="server">
 <h2>Async APM</h2>
 <p>Each task take 100ms to complete, but the overall time spent is under 200ms.</p>
 <h3>Chronology</h3>
 <uc:AsyncMessages runat="server" />
</asp:content>

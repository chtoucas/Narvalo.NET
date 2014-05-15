<%@ Page Async="true" Language="C#" CodeBehind="WidgetLookup.aspx.cs" Inherits="Playground.WebForms.WidgetLookupPage" %>

<%@ Register Src="~/Controls/LookupWidgetControl.ascx" TagPrefix="uc" TagName="LookupWidget" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <h1>Simple Data Demo: Lookup Widget</h1>
 <uc:LookupWidget runat="server" />
</asp:content>

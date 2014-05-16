<%@ Page Async="true" Language="C#" CodeBehind="WidgetLookup.aspx.cs" Inherits="Playground.WidgetLookupPage" %>

<%@ Register Src="~/Controls/LookupWidgetControl.ascx" TagPrefix="uc" TagName="LookupWidget" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <h2>Simple Data: Lookup Widget</h2>
 <uc:LookupWidget runat="server" />
</asp:content>

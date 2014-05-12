<%@ Page Async="true" Language="C#" MasterPageFile="~/Layouts/Site.Master" CodeBehind="WidgetLookup.aspx.cs"
 Inherits="Playground.WebForms.WidgetLookupPage" %>

<%@ Register Src="~/Controls/LookupWidgetControl.ascx" TagPrefix="uc" TagName="LookupWidget" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
 <h1>Simple Data Demo: Lookup Widget</h1>
 <uc:LookupWidget runat="server" />
</asp:Content>

<%@ Page Language="C#" CodeBehind="Messaging.aspx.cs" Inherits="Playground.WebForms.MessagingPage" %>

<%@ Register Src="~/Controls/Messaging1Control.ascx" TagPrefix="uc" TagName="Messaging1" %>
<%@ Register Src="~/Controls/Messaging2Control.ascx" TagPrefix="uc" TagName="Messaging2" %>

<asp:content contentplaceholderid="MainContent" runat="server">
 <h2>Cross-Presenter Communication</h2>
 <uc:Messaging1 runat="server" />
 <uc:Messaging2 runat="server" />
</asp:content>
